using System;
using System.Security.Cryptography;
using System.Text;

namespace pmp_client_cli
{
    public class PassData
    {
        private const string LengthFlag = "-l";
        private const string NoSpecialFlag = "-ns";
        private const string NoNumericFlag = "-nn";
        private const string NoAlphaFlag = "-na";
        private const string WhitelistFlag = "-wl";
        private const string BlacklistFlag = "-bl";
        private const int MinLength = 5;
        private const int MaxLength = 30;
        
        private int _length;
        private bool _init;
        private bool _alpha;
        private bool _numerics;
        private bool _special;
        private bool _isBlacklist;
        private bool _isWhitelist;
        private string _blacklist;
        private string _whitelist;
        private string _password;

        public PassData()
        {
            _length = 25;
            _init = false;
            _alpha = true;
            _numerics = true;
            _special = true;
            _isBlacklist = false;
            _isWhitelist = false;
            _blacklist = "";
            _whitelist = "";
            _password = "";
        }

        public bool Init(string[] args)
        {
            try
            {
                foreach (string t in args)
                {
                    if (t.Contains(LengthFlag))
                    {
                        string temp = t.Split('=')[1];
                        if (!int.TryParse(temp, out _length))
                            throw new Exception("Invalid length argument provided.");
                        if (_length < MinLength || _length > MaxLength)
                            throw new Exception(String.Format("Invalid length. The software supports " +
                                                              "lengths between {0} and {1}.", MinLength, MaxLength));
                    }
                    else if (t.Contains(NoSpecialFlag))
                        _special = false;
                    else if (t.Contains(NoNumericFlag))
                        _numerics = false;
                    else if (t.Contains(NoAlphaFlag))
                        _alpha = false;
                    else if (t.Contains(WhitelistFlag))
                    {
                        _whitelist = t.Split('=')[1];
                        if (_whitelist == "")
                            throw new Exception("Invalid whitelist provided.");

                        _isWhitelist = true;
                    }
                    else if (t.Contains(BlacklistFlag))
                    {
                        _blacklist = t.Split('=')[1];
                        if (_blacklist == "")
                            throw new Exception("Invalid blacklist provided.");

                        _isBlacklist = true;
                    }
                }

                _init = true;
                return _init;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error parsing command-line arguments" + e.Message);
                return false;
            }
        }

        public string GeneratePass()
        {
            string valid = "";
            try
            {
                if (!_init)
                    throw new Exception("Data object not initialized.");

                _password = "";

                if (_isWhitelist)
                    valid = _whitelist;
                else
                {
                    if (_alpha)
                        valid += "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    if (_numerics)
                        valid += "1234567890";
                    if (_special)
                        valid += " !#$%^&*()-+_=[]{}|;:,./<>?`~";
                    if (_isBlacklist)
                    {
                        for (int i = 0; i < _blacklist.Length; i++)
                        {
                            valid = valid.Replace(_blacklist[i].ToString(), "");
                        }
                    }
                }

                if (valid.Length < MinLength)
                    throw new Exception("Not enough of a valid character pool provided.");

                _password = GenerateRandom(valid);
                return _password;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while generating password: " + e.Message);
                throw;
            }
        }

        //TODO: Not truly random. Investigate later.
        private string GenerateRandom(string valid)
        {
            try
            {
                StringBuilder res = new StringBuilder();
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    byte[] uintBuffer = new byte[sizeof(uint)];

                    while (_length-- > 0)
                    {
                        rng.GetBytes(uintBuffer);
                        uint num = BitConverter.ToUInt32(uintBuffer, 0);
                        res.Append(valid[(int)(num % (uint)valid.Length)]);
                    }
                }

                return res.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while generating random string: " + e.Message);
                throw;
            }
        }
    }
}