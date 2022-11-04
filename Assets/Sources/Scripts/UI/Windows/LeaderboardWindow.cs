using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Assets.Scripts.Social.YandexSocialAdapter;

namespace Assets.Scripts.UI
{
    public class LeaderboardWindow : AbstractWindow
    {
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private RectTransform _viewsContainer;
        [Space]
        [SerializeField] private LeaderboardPlayerView _playerViewPrefab;

        public const string DefaultLeaderBoardName = "LevelValue";
        public override string LockKey => "LeaderboardWindow";

        public override bool AnimatedClose => true;

        public static LeaderboardWindow Show() =>
                        Game.Windows.ScreenChange<LeaderboardWindow>(false, w => w.Init());

        protected void Init()
        {
            SetText();

            Game.Windows.SoftLoader.gameObject.SetActive(true);
           Game.SocialAdapter.GetLeaderboardData(OnLeaderboardResponce, DefaultLeaderBoardName);
        }

        protected override void SetText()
        {
            _titleText.text = "leaderboard".Localize();
        }

        private void OnLeaderboardResponce(List<LeaderboardData> data)
        {
            if (gameObject == null)
                return;

            if(data == null)
            {
                Game.Windows.SoftLoader.gameObject.SetActive(false);
                Close();
            }
            else
            {
                data.ForEach(user => CreatePlayerView(user));
                Game.Windows.Loader.gameObject.SetActive(false);
            }
        }

        private void CreatePlayerView(LeaderboardData user)
        {
            LeaderboardPlayerView view = Instantiate(_playerViewPrefab, _viewsContainer);
            view.Init(user.UserName, user.ScoreValue);
        }
    }
}