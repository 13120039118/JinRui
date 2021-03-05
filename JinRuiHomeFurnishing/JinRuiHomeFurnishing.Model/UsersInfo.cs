using System;

namespace JinRuiHomeFurnishing.Model
{
	/// <summary>
	/// <para>UsersInfo Object</para>
	/// <para>Summary description for UsersInfo.</para>
	/// <para><see cref="member"/></para>
	/// <remarks></remarks>
	/// </summary>
	[Serializable]
	public class UsersInfo
	{
        #region Enum
        public enum Fields
        {
            /// <summary>
			/// 
			/// </summary>
			UserId,
			/// <summary>
			/// 
			/// </summary>
			UserName,
			/// <summary>
			/// 
			/// </summary>
			Password,
			/// <summary>
			/// 
			/// </summary>
			PasswordSalt,
			/// <summary>
			/// 
			/// </summary>
			CommonName,
			/// <summary>
			/// 
			/// </summary>
			Country,
			/// <summary>
			/// 
			/// </summary>
			Province,
			/// <summary>
			/// 
			/// </summary>
			City,
			/// <summary>
			/// 
			/// </summary>
			CooperationType,
			/// <summary>
			/// 
			/// </summary>
			CooperationID,
			/// <summary>
			/// 
			/// </summary>
			AccountAmount,
			/// <summary>
			/// 
			/// </summary>
			JiFenAmount,
			/// <summary>
			/// 
			/// </summary>
			MSN,
			/// <summary>
			/// 
			/// </summary>
			QQ,
			/// <summary>
			/// 
			/// </summary>
			PostalCode,
			/// <summary>
			/// 
			/// </summary>
			Phone,
			/// <summary>
			/// 
			/// </summary>
			Mobile,
			/// <summary>
			/// 
			/// </summary>
			Email,
			/// <summary>
			/// 
			/// </summary>
			Address,
			/// <summary>
			/// 
			/// </summary>
			OriginUrl,
			/// <summary>
			/// 
			/// </summary>
			CreateDate,
			/// <summary>
			/// 
			/// </summary>
			LastUpdateDate,
			/// <summary>
			/// 
			/// </summary>
			IsLockedOut,
			/// <summary>
			/// 
			/// </summary>
			IpAddress,
			/// <summary>
			/// 
			/// </summary>
			QQAccessToken,
			/// <summary>
			/// 
			/// </summary>
			SinaAccessToken,
			/// <summary>
			/// 
			/// </summary>
			Company,
			/// <summary>
			/// 
			/// </summary>
			Area,
			/// <summary>
			/// 
			/// </summary>
			ImageUrl,
			/// <summary>
			/// 
			/// </summary>
			Sex,
			/// <summary>
			/// 
			/// </summary>
			LikeClass,
			/// <summary>
			/// 
			/// </summary>
			NickName,
			/// <summary>
			/// 
			/// </summary>
			IsAuthMobile,
			/// <summary>
			/// 
			/// </summary>
			IsAuthEmail,
			/// <summary>
			/// 
			/// </summary>
			PageType,
			/// <summary>
			/// 
			/// </summary>
			LastLoginTime,
			/// <summary>
			/// 
			/// </summary>
			Source,
			/// <summary>
			/// 
			/// </summary>
			Pictrue,
			/// <summary>
			/// 
			/// </summary>
			JianJie,
			/// <summary>
			/// 
			/// </summary>
			QianMing,
			/// <summary>
			/// 
			/// </summary>
			LastQianDaoTime,
			/// <summary>
			/// 
			/// </summary>
			QianDaoCount,
			/// <summary>
			/// 
			/// </summary>
			YeWuMobile,
			/// <summary>
			/// 
			/// </summary>
			YeWuEmail,
			/// <summary>
			/// 
			/// </summary>
			YeWuQQ,
			/// <summary>
			/// 
			/// </summary>
			WeiXinHao,
			/// <summary>
			/// 
			/// </summary>
			MobileProvince,
			/// <summary>
			/// 
			/// </summary>
			MobileCity,
			/// <summary>
			/// 
			/// </summary>
			IsCrmHiden,
			/// <summary>
			/// 
			/// </summary>
			IsHiden,
			/// <summary>
			/// 
			/// </summary>
			UserTableId,
			/// <summary>
			/// 
			/// </summary>
			LastJiFenBalanceTime,
			/// <summary>
			/// 
			/// </summary>
			LoginCount,
			/// <summary>
			/// 
			/// </summary>
			JinJiDianHua,
			/// <summary>
			/// 
			/// </summary>
			ShenFenZheng,
			/// <summary>
			/// 
			/// </summary>
			XueLi,
			/// <summary>
			/// 
			/// </summary>
			QiTaGanXingQuKeCheng,
			/// <summary>
			/// 
			/// </summary>
			YaoQingMa,
			/// <summary>
			/// 
			/// </summary>
			DaQuId,
			/// <summary>
			/// 
			/// </summary>
			XiaoQuId,
			/// <summary>
			/// 
			/// </summary>
			GongZuoNianXian,
			/// <summary>
			/// 
			/// </summary>
			BaoKaoKeMu,
			/// <summary>
			/// 
			/// </summary>
			ZhuanYe,
			/// <summary>
			/// 
			/// </summary>
			IsXueYuan,
			/// <summary>
			/// 
			/// </summary>
			KaiKeShiJian,
			/// <summary>
			/// 
			/// </summary>
			SiteIndex,
			/// <summary>
			/// 
			/// </summary>
			XinXiLiu,
			/// <summary>
			/// 
			/// </summary>
			ZhiWei,
			/// <summary>
			/// 
			/// </summary>
			RuZhiTime,
			/// <summary>
			/// 
			/// </summary>
			ZhuanZhengTime,
			/// <summary>
			/// 
			/// </summary>
			LiZhiTime,
			/// <summary>
			/// 
			/// </summary>
			ZpFuZheRen,
			/// <summary>
			/// 
			/// </summary>
			PxFuZheRen,
			/// <summary>
			/// 
			/// </summary>
			JieShaoRen,
			/// <summary>
			/// 
			/// </summary>
			BeiZhu,
			/// <summary>
			/// 
			/// </summary>
			MobileYuanGong,
			/// <summary>
			/// 
			/// </summary>
			RenGongChengBen,
			/// <summary>
			/// 
			/// </summary>
			IsYuanGong,
			/// <summary>
			/// 
			/// </summary>
			CreateUserTableId,
			/// <summary>
			/// 
			/// </summary>
			GroupId,
			/// <summary>
			/// 
			/// </summary>
			IsDaoChuCheck,
			/// <summary>
			/// 
			/// </summary>
			DaoChuCheckTime,
			/// <summary>
			/// 
			/// </summary>
			TargetYuanXiao,
			/// <summary>
			/// 
			/// </summary>
			Age,
			/// <summary>
			/// 
			/// </summary>
			IsPeiXun,
			/// <summary>
			/// 
			/// </summary>
			AppSource,
			/// <summary>
			/// 
			/// </summary>
			Birthday,
			/// <summary>
			/// 
			/// </summary>
			QuDao,
			/// <summary>
			/// 
			/// </summary>
			TuiGuangFangShi,
			/// <summary>
			/// 
			/// </summary>
			CreateUserGroupId,
			/// <summary>
			/// 
			/// </summary>
			QuDaoBaoTableId,
			/// <summary>
			/// 
			/// </summary>
			QQOpenId,
			/// <summary>
			/// 
			/// </summary>
			WeiXinOpenId,
			/// <summary>
			/// 
			/// </summary>
			WeiBoOpenId,
			/// <summary>
			/// 
			/// </summary>
			RawURL,
			/// <summary>
			/// 意向院校
			/// </summary>
			IntentionCollege,
			/// <summary>
			/// 身份
			/// </summary>
			ShenFen,
			/// <summary>
			/// 意向词ID
			/// </summary>
			IntentionId
        }
        #endregion

        #region Fields
        private Guid _userId = Guid.Empty;
        private string _userName = String.Empty;
		private string _password = String.Empty;
		private string _passwordSalt = String.Empty;
		private string _commonName = String.Empty;
		private int _country;
		private int _province;
		private int _city;
		private int _cooperationType;
		private int _cooperationID;
		private decimal _accountAmount;
		private decimal _jiFenAmount;
		private string _mSN = String.Empty;
		private string _qQ = String.Empty;
		private string _postalCode = String.Empty;
		private string _phone = String.Empty;
        private string _mobile;
        private string _email = String.Empty;
		private string _address = String.Empty;
		private string _originUrl = String.Empty;
		private DateTime _createDate;
		private DateTime _lastUpdateDate;
        private bool _isLockedOut;
        private string _ipAddress = String.Empty;
		private string _qQAccessToken = String.Empty;
		private string _sinaAccessToken = String.Empty;
		private string _company = String.Empty;
		private int _area;
		private string _imageUrl = String.Empty;
		private string _sex = String.Empty;
		private int _likeClass;
		private string _nickName = String.Empty;
		private bool _isAuthMobile;
		private bool _isAuthEmail;
		private int _pageType;
		private DateTime _lastLoginTime;
		private int _source;
		private string _pictrue = String.Empty;
		private string _jianJie = String.Empty;
		private string _qianMing = String.Empty;
		private DateTime _lastQianDaoTime;
		private int _qianDaoCount;
		private string _yeWuMobile = String.Empty;
		private string _yeWuEmail = String.Empty;
		private string _yeWuQQ = String.Empty;
		private string _weiXinHao = String.Empty;
		private int _mobileProvince;
		private int _mobileCity;
		private bool _isCrmHiden;
		private bool _isHiden;
		private int _userTableId;
		private DateTime _lastJiFenBalanceTime;
		private int _loginCount;
		private string _jinJiDianHua = String.Empty;
		private string _shenFenZheng = String.Empty;
		private int _xueLi;
		private string _qiTaGanXingQuKeCheng = String.Empty;
		private string _yaoQingMa = String.Empty;
		private int _daQuId;
		private int _xiaoQuId;
		private int _gongZuoNianXian;
		private int _baoKaoKeMu;
		private string _zhuanYe = String.Empty;
		private int _isXueYuan;
		private DateTime _kaiKeShiJian;
		private int _siteIndex;
		private int _xinXiLiu;
		private string _zhiWei = String.Empty;
		private DateTime _ruZhiTime;
		private DateTime _zhuanZhengTime;
		private DateTime _liZhiTime;
		private string _zpFuZheRen = String.Empty;
		private string _pxFuZheRen = String.Empty;
		private string _jieShaoRen = String.Empty;
		private string _beiZhu = String.Empty;
		private string _mobileYuanGong = String.Empty;
		private string _renGongChengBen = String.Empty;
		private int _isYuanGong;
		private int _createUserTableId;
		private int _groupId;
		private int _isDaoChuCheck;
		private DateTime _daoChuCheckTime;
		private int _targetYuanXiao;
		private int _age;
		private int _isPeiXun;
		private int _appSource;
		private DateTime _birthday;
		private int _quDao;
		private int _tuiGuangFangShi;
		private int _createUserGroupId;
		private int _quDaoBaoTableId;
		private string _qQOpenId = String.Empty;
		private string _weiXinOpenId = String.Empty;
		private string _weiBoOpenId = String.Empty;
		private string _rawURL = String.Empty;
		private int _intentionCollege;
		private int _shenFen;
		private int _intentionId;
		#endregion
		
		#region Contructors
		public UsersInfo()
		{
		}
		
		public UsersInfo
		(
            Guid userId,
            string userName,
			string password,
			string passwordSalt,
			string commonName,
			int country,
			int province,
			int city,
			int cooperationType,
			int cooperationID,
			decimal accountAmount,
			decimal jiFenAmount,
			string mSN,
			string qQ,
			string postalCode,
			string phone,
			string mobile,
			string email,
			string address,
			string originUrl,
			DateTime createDate,
			DateTime lastUpdateDate,
            bool isLockedOut,
			string ipAddress,
			string qQAccessToken,
			string sinaAccessToken,
			string company,
			int area,
			string imageUrl,
			string sex,
			int likeClass,
			string nickName,
            bool isAuthMobile,
            bool isAuthEmail,
			int pageType,
			DateTime lastLoginTime,
			int source,
			string pictrue,
			string jianJie,
			string qianMing,
			DateTime lastQianDaoTime,
			int qianDaoCount,
			string yeWuMobile,
			string yeWuEmail,
			string yeWuQQ,
			string weiXinHao,
			int mobileProvince,
			int mobileCity,
            bool isCrmHiden,
            bool isHiden,
            int userTableId,
            DateTime lastJiFenBalanceTime,
			int loginCount,
			string jinJiDianHua,
			string shenFenZheng,
			int xueLi,
			string qiTaGanXingQuKeCheng,
			string yaoQingMa,
			int daQuId,
			int xiaoQuId,
			int gongZuoNianXian,
			int baoKaoKeMu,
			string zhuanYe,
			int isXueYuan,
			DateTime kaiKeShiJian,
			int siteIndex,
			int xinXiLiu,
			string zhiWei,
			DateTime ruZhiTime,
			DateTime zhuanZhengTime,
			DateTime liZhiTime,
			string zpFuZheRen,
			string pxFuZheRen,
			string jieShaoRen,
			string beiZhu,
			string mobileYuanGong,
			string renGongChengBen,
			int isYuanGong,
			int createUserTableId,
			int groupId,
			int isDaoChuCheck,
			DateTime daoChuCheckTime,
			int targetYuanXiao,
			int age,
			int isPeiXun,
			int appSource,
			DateTime birthday,
			int quDao,
			int tuiGuangFangShi,
			int createUserGroupId,
			int quDaoBaoTableId,
			string qQOpenId,
			string weiXinOpenId,
			string weiBoOpenId,
			string rawURL,
			int intentionCollege,
			int shenFen,
			int intentionId
		)
		{
			_userId               = userId;
			_userName             = userName;
			_password             = password;
			_passwordSalt         = passwordSalt;
			_commonName           = commonName;
			_country              = country;
			_province             = province;
			_city                 = city;
			_cooperationType      = cooperationType;
			_cooperationID        = cooperationID;
			_accountAmount        = accountAmount;
			_jiFenAmount          = jiFenAmount;
			_mSN                  = mSN;
			_qQ                   = qQ;
			_postalCode           = postalCode;
			_phone                = phone;
			_mobile               = mobile;
			_email                = email;
			_address              = address;
			_originUrl            = originUrl;
			_createDate           = createDate;
			_lastUpdateDate       = lastUpdateDate;
			_isLockedOut          = isLockedOut;
			_ipAddress            = ipAddress;
			_qQAccessToken        = qQAccessToken;
			_sinaAccessToken      = sinaAccessToken;
			_company              = company;
			_area                 = area;
			_imageUrl             = imageUrl;
			_sex                  = sex;
			_likeClass            = likeClass;
			_nickName             = nickName;
			_isAuthMobile         = isAuthMobile;
			_isAuthEmail          = isAuthEmail;
			_pageType             = pageType;
			_lastLoginTime        = lastLoginTime;
			_source               = source;
			_pictrue              = pictrue;
			_jianJie              = jianJie;
			_qianMing             = qianMing;
			_lastQianDaoTime      = lastQianDaoTime;
			_qianDaoCount         = qianDaoCount;
			_yeWuMobile           = yeWuMobile;
			_yeWuEmail            = yeWuEmail;
			_yeWuQQ               = yeWuQQ;
			_weiXinHao            = weiXinHao;
			_mobileProvince       = mobileProvince;
			_mobileCity           = mobileCity;
			_isCrmHiden           = isCrmHiden;
			_isHiden              = isHiden;
			_userTableId          = userTableId;
			_lastJiFenBalanceTime = lastJiFenBalanceTime;
			_loginCount           = loginCount;
			_jinJiDianHua         = jinJiDianHua;
			_shenFenZheng         = shenFenZheng;
			_xueLi                = xueLi;
			_qiTaGanXingQuKeCheng = qiTaGanXingQuKeCheng;
			_yaoQingMa            = yaoQingMa;
			_daQuId               = daQuId;
			_xiaoQuId             = xiaoQuId;
			_gongZuoNianXian      = gongZuoNianXian;
			_baoKaoKeMu           = baoKaoKeMu;
			_zhuanYe              = zhuanYe;
			_isXueYuan            = isXueYuan;
			_kaiKeShiJian         = kaiKeShiJian;
			_siteIndex            = siteIndex;
			_xinXiLiu             = xinXiLiu;
			_zhiWei               = zhiWei;
			_ruZhiTime            = ruZhiTime;
			_zhuanZhengTime       = zhuanZhengTime;
			_liZhiTime            = liZhiTime;
			_zpFuZheRen           = zpFuZheRen;
			_pxFuZheRen           = pxFuZheRen;
			_jieShaoRen           = jieShaoRen;
			_beiZhu               = beiZhu;
			_mobileYuanGong       = mobileYuanGong;
			_renGongChengBen      = renGongChengBen;
			_isYuanGong           = isYuanGong;
			_createUserTableId    = createUserTableId;
			_groupId              = groupId;
			_isDaoChuCheck        = isDaoChuCheck;
			_daoChuCheckTime      = daoChuCheckTime;
			_targetYuanXiao       = targetYuanXiao;
			_age                  = age;
			_isPeiXun             = isPeiXun;
			_appSource            = appSource;
			_birthday             = birthday;
			_quDao                = quDao;
			_tuiGuangFangShi      = tuiGuangFangShi;
			_createUserGroupId    = createUserGroupId;
			_quDaoBaoTableId      = quDaoBaoTableId;
			_qQOpenId             = qQOpenId;
			_weiXinOpenId         = weiXinOpenId;
			_weiBoOpenId          = weiBoOpenId;
			_rawURL               = rawURL;
			_intentionCollege     = intentionCollege;
			_shenFen              = shenFen;
			_intentionId          = intentionId;
			
		}
		#endregion
		
		#region Public Properties
		
		public Guid UserId
		{
			get {return _userId;}
			set {_userId = value;}
		}

		public string UserName
		{
			get {return _userName;}
			set {_userName = value;}
		}

		public string Password
		{
			get {return _password;}
			set {_password = value;}
		}

		public string PasswordSalt
		{
			get {return _passwordSalt;}
			set {_passwordSalt = value;}
		}

		public string CommonName
		{
			get {return _commonName;}
			set {_commonName = value;}
		}

		public int Country
		{
			get {return _country;}
			set {_country = value;}
		}

		public int Province
		{
			get {return _province;}
			set {_province = value;}
		}

		public int City
		{
			get {return _city;}
			set {_city = value;}
		}

		public int CooperationType
		{
			get {return _cooperationType;}
			set {_cooperationType = value;}
		}

		public int CooperationID
		{
			get {return _cooperationID;}
			set {_cooperationID = value;}
		}

		public decimal AccountAmount
		{
			get {return _accountAmount;}
			set {_accountAmount = value;}
		}

		public decimal JiFenAmount
		{
			get {return _jiFenAmount;}
			set {_jiFenAmount = value;}
		}

		public string MSN
		{
			get {return _mSN;}
			set {_mSN = value;}
		}

		public string QQ
		{
			get {return _qQ;}
			set {_qQ = value;}
		}

		public string PostalCode
		{
			get {return _postalCode;}
			set {_postalCode = value;}
		}

		public string Phone
		{
			get {return _phone;}
			set {_phone = value;}
		}

		public string Mobile
		{
			get {return _mobile;}
			set {_mobile = value;}
		}

		public string Email
		{
			get {return _email;}
			set {_email = value;}
		}

		public string Address
		{
			get {return _address;}
			set {_address = value;}
		}

		public string OriginUrl
		{
			get {return _originUrl;}
			set {_originUrl = value;}
		}

		public DateTime CreateDate
		{
			get {return _createDate;}
			set {_createDate = value;}
		}

		public DateTime LastUpdateDate
		{
			get {return _lastUpdateDate;}
			set {_lastUpdateDate = value;}
		}

		public bool IsLockedOut
		{
			get {return _isLockedOut;}
			set {_isLockedOut = value;}
		}

		public string IpAddress
		{
			get {return _ipAddress;}
			set {_ipAddress = value;}
		}

		public string QQAccessToken
		{
			get {return _qQAccessToken;}
			set {_qQAccessToken = value;}
		}

		public string SinaAccessToken
		{
			get {return _sinaAccessToken;}
			set {_sinaAccessToken = value;}
		}

		public string Company
		{
			get {return _company;}
			set {_company = value;}
		}

		public int Area
		{
			get {return _area;}
			set {_area = value;}
		}

		public string ImageUrl
		{
			get {return _imageUrl;}
			set {_imageUrl = value;}
		}

		public string Sex
		{
			get {return _sex;}
			set {_sex = value;}
		}

		public int LikeClass
		{
			get {return _likeClass;}
			set {_likeClass = value;}
		}

		public string NickName
		{
			get {return _nickName;}
			set {_nickName = value;}
		}

		public bool IsAuthMobile
		{
			get {return _isAuthMobile;}
			set {_isAuthMobile = value;}
		}

		public bool IsAuthEmail
		{
			get {return _isAuthEmail;}
			set {_isAuthEmail = value;}
		}

		public int PageType
		{
			get {return _pageType;}
			set {_pageType = value;}
		}

		public DateTime LastLoginTime
		{
			get {return _lastLoginTime;}
			set {_lastLoginTime = value;}
		}

		public int Source
		{
			get {return _source;}
			set {_source = value;}
		}

		public string Pictrue
		{
			get {return _pictrue;}
			set {_pictrue = value;}
		}

		public string JianJie
		{
			get {return _jianJie;}
			set {_jianJie = value;}
		}

		public string QianMing
		{
			get {return _qianMing;}
			set {_qianMing = value;}
		}

		public DateTime LastQianDaoTime
		{
			get {return _lastQianDaoTime;}
			set {_lastQianDaoTime = value;}
		}

		public int QianDaoCount
		{
			get {return _qianDaoCount;}
			set {_qianDaoCount = value;}
		}

		public string YeWuMobile
		{
			get {return _yeWuMobile;}
			set {_yeWuMobile = value;}
		}

		public string YeWuEmail
		{
			get {return _yeWuEmail;}
			set {_yeWuEmail = value;}
		}

		public string YeWuQQ
		{
			get {return _yeWuQQ;}
			set {_yeWuQQ = value;}
		}

		public string WeiXinHao
		{
			get {return _weiXinHao;}
			set {_weiXinHao = value;}
		}

		public int MobileProvince
		{
			get {return _mobileProvince;}
			set {_mobileProvince = value;}
		}

		public int MobileCity
		{
			get {return _mobileCity;}
			set {_mobileCity = value;}
		}

		public bool IsCrmHiden
		{
			get {return _isCrmHiden;}
			set {_isCrmHiden = value;}
		}

		public bool IsHiden
		{
			get {return _isHiden;}
			set {_isHiden = value;}
		}

		public int UserTableId
		{
			get {return _userTableId;}
			set {_userTableId = value;}
		}

		public DateTime LastJiFenBalanceTime
		{
			get {return _lastJiFenBalanceTime;}
			set {_lastJiFenBalanceTime = value;}
		}

		public int LoginCount
		{
			get {return _loginCount;}
			set {_loginCount = value;}
		}

		public string JinJiDianHua
		{
			get {return _jinJiDianHua;}
			set {_jinJiDianHua = value;}
		}

		public string ShenFenZheng
		{
			get {return _shenFenZheng;}
			set {_shenFenZheng = value;}
		}

		public int XueLi
		{
			get {return _xueLi;}
			set {_xueLi = value;}
		}

		public string QiTaGanXingQuKeCheng
		{
			get {return _qiTaGanXingQuKeCheng;}
			set {_qiTaGanXingQuKeCheng = value;}
		}

		public string YaoQingMa
		{
			get {return _yaoQingMa;}
			set {_yaoQingMa = value;}
		}

		public int DaQuId
		{
			get {return _daQuId;}
			set {_daQuId = value;}
		}

		public int XiaoQuId
		{
			get {return _xiaoQuId;}
			set {_xiaoQuId = value;}
		}

		public int GongZuoNianXian
		{
			get {return _gongZuoNianXian;}
			set {_gongZuoNianXian = value;}
		}

		public int BaoKaoKeMu
		{
			get {return _baoKaoKeMu;}
			set {_baoKaoKeMu = value;}
		}

		public string ZhuanYe
		{
			get {return _zhuanYe;}
			set {_zhuanYe = value;}
		}

		public int IsXueYuan
		{
			get {return _isXueYuan;}
			set {_isXueYuan = value;}
		}

		public DateTime KaiKeShiJian
		{
			get {return _kaiKeShiJian;}
			set {_kaiKeShiJian = value;}
		}

		public int SiteIndex
		{
			get {return _siteIndex;}
			set {_siteIndex = value;}
		}

		public int XinXiLiu
		{
			get {return _xinXiLiu;}
			set {_xinXiLiu = value;}
		}

		public string ZhiWei
		{
			get {return _zhiWei;}
			set {_zhiWei = value;}
		}

		public DateTime RuZhiTime
		{
			get {return _ruZhiTime;}
			set {_ruZhiTime = value;}
		}

		public DateTime ZhuanZhengTime
		{
			get {return _zhuanZhengTime;}
			set {_zhuanZhengTime = value;}
		}

		public DateTime LiZhiTime
		{
			get {return _liZhiTime;}
			set {_liZhiTime = value;}
		}

		public string ZpFuZheRen
		{
			get {return _zpFuZheRen;}
			set {_zpFuZheRen = value;}
		}

		public string PxFuZheRen
		{
			get {return _pxFuZheRen;}
			set {_pxFuZheRen = value;}
		}

		public string JieShaoRen
		{
			get {return _jieShaoRen;}
			set {_jieShaoRen = value;}
		}

		public string BeiZhu
		{
			get {return _beiZhu;}
			set {_beiZhu = value;}
		}

		public string MobileYuanGong
		{
			get {return _mobileYuanGong;}
			set {_mobileYuanGong = value;}
		}

		public string RenGongChengBen
		{
			get {return _renGongChengBen;}
			set {_renGongChengBen = value;}
		}

		public int IsYuanGong
		{
			get {return _isYuanGong;}
			set {_isYuanGong = value;}
		}

		public int CreateUserTableId
		{
			get {return _createUserTableId;}
			set {_createUserTableId = value;}
		}

		public int GroupId
		{
			get {return _groupId;}
			set {_groupId = value;}
		}

		public int IsDaoChuCheck
		{
			get {return _isDaoChuCheck;}
			set {_isDaoChuCheck = value;}
		}

		public DateTime DaoChuCheckTime
		{
			get {return _daoChuCheckTime;}
			set {_daoChuCheckTime = value;}
		}

		public int TargetYuanXiao
		{
			get {return _targetYuanXiao;}
			set {_targetYuanXiao = value;}
		}

		public int Age
		{
			get {return _age;}
			set {_age = value;}
		}

		public int IsPeiXun
		{
			get {return _isPeiXun;}
			set {_isPeiXun = value;}
		}

		public int AppSource
		{
			get {return _appSource;}
			set {_appSource = value;}
		}

		public DateTime Birthday
		{
			get {return _birthday;}
			set {_birthday = value;}
		}

		public int QuDao
		{
			get {return _quDao;}
			set {_quDao = value;}
		}

		public int TuiGuangFangShi
		{
			get {return _tuiGuangFangShi;}
			set {_tuiGuangFangShi = value;}
		}

		public int CreateUserGroupId
		{
			get {return _createUserGroupId;}
			set {_createUserGroupId = value;}
		}

		public int QuDaoBaoTableId
		{
			get {return _quDaoBaoTableId;}
			set {_quDaoBaoTableId = value;}
		}

		public string QQOpenId
		{
			get {return _qQOpenId;}
			set {_qQOpenId = value;}
		}

		public string WeiXinOpenId
		{
			get {return _weiXinOpenId;}
			set {_weiXinOpenId = value;}
		}

		public string WeiBoOpenId
		{
			get {return _weiBoOpenId;}
			set {_weiBoOpenId = value;}
		}

		public string RawURL
		{
			get {return _rawURL;}
			set {_rawURL = value;}
		}

        /// <summary>
        /// 意向院校
        /// </summary>
		public int IntentionCollege
		{
			get {return _intentionCollege;}
			set {_intentionCollege = value;}
		}

        /// <summary>
        /// 身份
        /// </summary>
		public int ShenFen
		{
			get {return _shenFen;}
			set {_shenFen = value;}
		}

        /// <summary>
        /// 意向词ID
        /// </summary>
		public int IntentionId
		{
			get {return _intentionId;}
			set {_intentionId = value;}
		}
		#endregion
	}
}