using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_BPC.hrfocus.utility
{
    public class cls_Hash
    {
        #region Private member variables...

		private string mSalt;
		private HashAlgorithm mCryptoService;

		#endregion

		#region Public interfaces...

		public enum ServiceProviderEnum: int
		{
			// Supported algorithms
			SHA1, 
			SHA256, 
			SHA384, 
			SHA512, 
			MD5
		}

		public cls_Hash()
		{
			// Default Hash algorithm
			mCryptoService = new SHA1Managed();
		}

		public cls_Hash(ServiceProviderEnum serviceProvider)
		{	
			// Select hash algorithm
			switch(serviceProvider)
			{
				case ServiceProviderEnum.MD5:
					mCryptoService = new MD5CryptoServiceProvider();
					break;
				case ServiceProviderEnum.SHA1:
					mCryptoService = new SHA1Managed();
					break;
				case ServiceProviderEnum.SHA256:
					mCryptoService = new SHA256Managed();
					break;
				case ServiceProviderEnum.SHA384:
					mCryptoService = new SHA384Managed();
					break;
				case ServiceProviderEnum.SHA512:
					mCryptoService = new SHA512Managed();
					break;
			}
		}

        public cls_Hash(string serviceProviderName)
		{
			try
			{
				// Set Hash algorithm
				mCryptoService = (HashAlgorithm)CryptoConfig.CreateFromName(serviceProviderName.ToUpper());
			}
			catch
			{
				throw;
			}
		}

		public virtual string Encrypt(string plainText)
		{
			byte[] cryptoByte =  mCryptoService.ComputeHash(ASCIIEncoding.ASCII.GetBytes(plainText + mSalt));
			
			// Convert into base 64 to enable result to be used in Xml
			return Convert.ToBase64String(cryptoByte, 0, cryptoByte.Length);
		}
		
		public string Salt
		{
			// Salt value
			get
			{
				return mSalt;
			}
			set
			{
				mSalt = value;
			}
		}
		#endregion
    }
}
