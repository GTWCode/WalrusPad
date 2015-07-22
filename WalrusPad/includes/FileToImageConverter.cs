﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace WalrusPad
{
	public class FileAssociation
	{
		// Associate file extension with progID, description, icon and application
		public static void Associate(string extension,
			   string progID, string description, string icon, string application) {
			Registry.CurrentUser.CreateSubKey(extension).SetValue("", progID);
			if (progID != null && progID.Length > 0)
				using (RegistryKey key = Registry.CurrentUser.CreateSubKey(progID)) {
					if (description != null)
						key.SetValue("", description);
					if (icon != null)
						key.CreateSubKey("DefaultIcon").SetValue("", ToShortPathName(icon));
					if (application != null)
						key.CreateSubKey(@"Shell\Open\Command").SetValue("",
									ToShortPathName(application) + " \"%1\"");
				}
		}

		// Return true if extension already associated in registry
		public static bool IsAssociated(string extension) {
			return (Registry.CurrentUser.OpenSubKey(extension, false) != null);
		}

		[DllImport("Kernel32.dll")]
		private static extern uint GetShortPathName(string lpszLongPath,
			[Out] StringBuilder lpszShortPath, uint cchBuffer);

		// Return short path format of a file name
		private static string ToShortPathName(string longName) {
			StringBuilder s = new StringBuilder(1000);
			uint iSize = (uint)s.Capacity;
			uint iRet = GetShortPathName(longName, s, iSize);
			return s.ToString();
		}
	}
}
