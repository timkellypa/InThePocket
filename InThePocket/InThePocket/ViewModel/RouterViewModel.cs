using System;
using System.Collections.Generic;
using InThePocket.Navigation;
using System.Threading.Tasks;

namespace InThePocket.ViewModel
{
    public class RouterViewModel : ViewModelBase
    {
        public override Guid GetCommToken()
        {
            return new Guid("B2DC2999-BD6A-4FFD-BD57-BAABA4B06AEE");
        }

        public override List<String> GetCommProperties()
        {
            return new List<String>() { "Route" };
        }

        public override Task ProcessArguments(List<String> arguments)
        {
            throw new NotImplementedException();
        }

        public RouterViewModel() : base() { }

        public Route Route { get; set; }
    }
}
