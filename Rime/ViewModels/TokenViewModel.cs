using Rime.ADO.Classes;

namespace Rime.ViewModels
{
    public class TokenViewModel
    {
        public Token Token { get; set; } = new Token();
        public Asset Asset { get; set; } = new Asset();

        public TokenViewModel(Token t)
        {
            Token = t;
        }

        public TokenViewModel() { }

        public Token ToToken()
            => new Token(this);

    }
}