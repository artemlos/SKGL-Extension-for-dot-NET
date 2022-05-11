﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SKM.V3.Methods;

namespace SKM_Test
{
    using SKGL;
    [TestClass]
    public class TestMachineCode
    {
        [TestMethod]
        public void UserNameMachineCodeTest()
        {
            string machineCode = SKM.getMachineCode(SKM.getSHA1);
            string machineCode2 =SKM.getMachineCode(SKM.getSHA1, true);

            Assert.AreNotEqual(machineCode, machineCode2);
        }

        [TestMethod]
        public void MachineCodeNew()
        {
            string mc = Helpers.GetMachineCodePI();
            string mc2 = Helpers.GetMachineCodePI(2);
        }
    }
}
