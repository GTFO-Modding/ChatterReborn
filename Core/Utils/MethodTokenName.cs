namespace ChatterReborn.Utils
{
    public struct MethodToken
    {


        public static implicit operator string(MethodToken token)
        {
            return token.m_name;
        }


        public static implicit operator uint(MethodToken token)
        {
            return token.m_tokenID;
        }


        public static implicit operator MethodToken(string name)
        {
            return new MethodToken(name);
        }


        public static implicit operator MethodToken(uint tokenID)
        {
            return new MethodToken(tokenID);
        }



        public MethodToken(uint tokenID)
        {
            this = default;
            m_tokenID  = tokenID;
        }

        public MethodToken(string name)
        {
            this = default;
            m_name  = name;
        }

        public MethodToken(string name, uint tokenID)
        {
            m_tokenID = tokenID;
            m_name  = name;
        }

        public bool HasToken => this.m_tokenID > 0;

        public string m_name;

        public uint m_tokenID;
    }
}
