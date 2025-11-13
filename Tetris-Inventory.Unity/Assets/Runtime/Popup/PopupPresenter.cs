using System.Threading.Tasks;

namespace Runtime.Popup
{
    public sealed class PopupPresenter
    {
        private readonly PopupModel _model;
        private readonly PopupView _popupView;
        private readonly float _visibleDuration;

        public PopupPresenter(PopupModel model,
            PopupView popupView,
            float visibleDuration = 1f)
        {
            _popupView = popupView;
            _model = model;
            _visibleDuration = visibleDuration;
        }

        public void Enable()
        {
            _model.OnPopupOpened += HandlePopupOpened;
            _model.OnPopupClosed += HandlePopupClosed;
        }
        
        public void Disable()
        {
            _model.OnPopupOpened -= HandlePopupOpened;
            _model.OnPopupClosed -= HandlePopupClosed;
        }

        private async void HandlePopupOpened(PopupData data)
        {
            _popupView.Title.text = data.Title;
            _popupView.Message.text = data.Message;
            
            _popupView.Show();
            
            await Task.Delay((int)(_visibleDuration * 1000));
            
            _popupView.Hide();
        }
        
        private void HandlePopupClosed(PopupData data)
        {
            _popupView.Hide();
        }
    }
}