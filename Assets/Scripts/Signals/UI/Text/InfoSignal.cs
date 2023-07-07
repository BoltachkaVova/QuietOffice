namespace Signals
{
    public struct InfoSignal
    {
        private readonly string _nameText;
        private readonly string _infoText;
        
        public string NameText => _nameText;
        public string InfoText => _infoText;

        public InfoSignal(string nameText, string infoText)
        {
            _nameText = nameText;
            _infoText = infoText;
        }
    }
}