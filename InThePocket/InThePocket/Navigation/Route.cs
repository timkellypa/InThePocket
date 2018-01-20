using System.Collections.Generic;

namespace InThePocket.Navigation
{

    public class Route
    {
        public string Page { get; set; }

        public List<string> Arguments { get; set; }

        public Route(string page, List<string> arguments)
        {
            Page = page;

            if (arguments == null)
            {
                Arguments = new List<string>();
            }
            else
            {
                Arguments = arguments;
            }
        }

        public Route(string uri)
        {
            Arguments = new List<string>();
            ParseRoute(uri);
        }

        private void ParseRoute(string uri)
        {
            string[] uriParts = uri.Split('/');
            Page = uriParts[0];
            for (int i = 1; i < uriParts.Length; ++i)
            {
                Arguments.Add(uriParts[i]);
            }
        }

        public override string ToString()
        {
            List<string> fullList = new List<string>() { Page };
            fullList.AddRange(Arguments);
            return string.Join("/", fullList);
        }
    }
}
