using System;
using System.Collections.Generic;
using DeviceProviders;
using OpenAlljoynExplorer.Models;
using OpenAlljoynExplorer.Support;

namespace OpenAlljoynExplorer.Controllers
{
    public class ServicePageController
    {
        private IService VM;

        public ServicePageController(IService VM)
        {
            this.VM = VM;
        }

        internal void Start()
        {

            //VM.Service
        }

    }
}