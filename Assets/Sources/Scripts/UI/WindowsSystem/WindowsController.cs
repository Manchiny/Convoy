using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class WindowsController : MonoBehaviour
    {
        [SerializeField] private RectTransform _windowHolder;
        [SerializeField] private Locker _locker;
        [SerializeField] private HUD _hud;
        [SerializeField] private RectTransform _loader;

        private Stack<AbstractWindow> _windows = new Stack<AbstractWindow>();

        public Locker Locker => _locker;
        public HUD HUD => _hud;
        public RectTransform Loader => _loader;
        private AbstractWindow CurrentWindow => _windows.Count > 0 ? _windows.Peek() : null;

        public T ScreenChange<T>(bool closeAllOther = true, Action<T> action = null) where T : AbstractWindow
        {
            if (CurrentWindow != null && CurrentWindow is T && CurrentWindow.IsClosing == false)
            {
                Debug.Log($"WindowsConntroller: rejected open window {CurrentWindow.LockKey}");
                return CurrentWindow as T;
            }

            T window = null;

            if (closeAllOther)
            {
                if (CurrentWindow != null)
                {
                    int counter = _windows.Count;
                    while (counter > 0)
                    {
                        CloseCurrentWindow();
                        counter--;
                    }
                }
            }
            else
            {
                if (CurrentWindow != null)
                    CurrentWindow.ForceHide();
            }

            window = OpenScreen(action);
            _windows.Push(window);

            if (window.NeedHideHudOnShow)
                _hud.Hide();
            else
                _hud.Show();

            return window;
        }

        private T OpenScreen<T>(Action<T> onWindowOpen, Dictionary<string, object> addLogParams = null) where T : AbstractWindow
        {
            var window = GetWindow<T>();

            if (window != null)
            {
                window = Instantiate(window, _windowHolder);
                onWindowOpen.Invoke(window);

                window.Closed += OnWindowClosed;
            }

            return window;
        }

        private void CloseCurrentWindow()
        {
            if (CurrentWindow != null)
            {
                Debug.Log($"Windows Controller close window: {CurrentWindow.LockKey}");
                CurrentWindow.Close();
            }
        }

        private T GetWindow<T>() where T : AbstractWindow
        {
            Type type = typeof(T);
            T window = null;

            if (WindowsHolder.Windows.ContainsKey(type))
            {
                window = Resources.Load<T>(WindowsHolder.Windows[type]);
            }

            return window;
        }

        private void OnWindowClosed(AbstractWindow window)
        {
            window.Closed -= OnWindowClosed;

            if (_windows.Count > 0)
            {
                if (CurrentWindow == window)
                    _windows.Pop();

                if (CurrentWindow != null)
                    CurrentWindow?.Unhide();
                else
                    _hud.Show();
            }

            if (_windows.Count == 0)
                _hud.Show();
        }
    }
}
