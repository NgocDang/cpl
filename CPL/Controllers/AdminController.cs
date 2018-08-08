using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CPL.Controllers
{
    [Permission(EnumRole.Admin)]
    public class AdminController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly IGameHistoryService _gameHistoryService;
        private readonly ITeamService _teamService;
        private readonly ITemplateService _templateService;
        private readonly ISysUserService _sysUserService;
        private readonly ILotteryHistoryService _lotteryHistoryService;
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;
        private readonly INewsService _newsService;
        private readonly IDictionary<string, string> countryDict = new Dictionary<string, string>();

        public AdminController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITeamService teamService,
            ITemplateService templateService,
            ISysUserService sysUserService,
            IGameHistoryService gameHistoryService,
            ILotteryHistoryService lotteryHistoryService,
            IPricePredictionHistoryService pricePredictionHistoryService,
            INewsService newsService)
        {
            this._langService = langService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._teamService = teamService;
            this._templateService = templateService;
            this._sysUserService = sysUserService;
            this._gameHistoryService = gameHistoryService;
            this._lotteryHistoryService = lotteryHistoryService;
            this._pricePredictionHistoryService = pricePredictionHistoryService;
            this._newsService = newsService;
            this.initCountryDict();
        }

        public void initCountryDict()
        {
            countryDict.Add(new KeyValuePair<string, string>("AF", "Afghanistan"));
            countryDict.Add(new KeyValuePair<string, string>("AX", "Åland Islands"));
            countryDict.Add(new KeyValuePair<string, string>("AL", "Albania"));
            countryDict.Add(new KeyValuePair<string, string>("DZ", "Algeria"));
            countryDict.Add(new KeyValuePair<string, string>("AS", "American Samoa"));
            countryDict.Add(new KeyValuePair<string, string>("AD", "Andorra"));
            countryDict.Add(new KeyValuePair<string, string>("AO", "Angola"));
            countryDict.Add(new KeyValuePair<string, string>("AI", "Anguilla"));
            countryDict.Add(new KeyValuePair<string, string>("AQ", "Antarctica"));
            countryDict.Add(new KeyValuePair<string, string>("AG", "Antigua and Barbuda"));
            countryDict.Add(new KeyValuePair<string, string>("AR", "Argentina"));
            countryDict.Add(new KeyValuePair<string, string>("AM", "Armenia"));
            countryDict.Add(new KeyValuePair<string, string>("AW", "Aruba"));
            countryDict.Add(new KeyValuePair<string, string>("AU", "Australia"));
            countryDict.Add(new KeyValuePair<string, string>("AT", "Austria"));
            countryDict.Add(new KeyValuePair<string, string>("AZ", "Azerbaijan"));
            countryDict.Add(new KeyValuePair<string, string>("BS", "Bahamas"));
            countryDict.Add(new KeyValuePair<string, string>("BH", "Bahrain"));
            countryDict.Add(new KeyValuePair<string, string>("BD", "Bangladesh"));
            countryDict.Add(new KeyValuePair<string, string>("BB", "Barbados"));
            countryDict.Add(new KeyValuePair<string, string>("BY", "Belarus"));
            countryDict.Add(new KeyValuePair<string, string>("BE", "Belgium"));
            countryDict.Add(new KeyValuePair<string, string>("BZ", "Belize"));
            countryDict.Add(new KeyValuePair<string, string>("BJ", "Benin"));
            countryDict.Add(new KeyValuePair<string, string>("BM", "Bermuda"));
            countryDict.Add(new KeyValuePair<string, string>("BT", "Bhutan"));
            countryDict.Add(new KeyValuePair<string, string>("BO", "Bolivia Plurinational State of"));
            countryDict.Add(new KeyValuePair<string, string>("BA", "Bosnia and Herzegovina"));
            countryDict.Add(new KeyValuePair<string, string>("BW", "Botswana"));
            countryDict.Add(new KeyValuePair<string, string>("BV", "Bouvet Island"));
            countryDict.Add(new KeyValuePair<string, string>("BR", "Brazil"));
            countryDict.Add(new KeyValuePair<string, string>("IO", "British Indian Ocean Territory"));
            countryDict.Add(new KeyValuePair<string, string>("BN", "Brunei Darussalam"));
            countryDict.Add(new KeyValuePair<string, string>("BG", "Bulgaria"));
            countryDict.Add(new KeyValuePair<string, string>("BF", "Burkina Faso"));
            countryDict.Add(new KeyValuePair<string, string>("BI", "Burundi"));
            countryDict.Add(new KeyValuePair<string, string>("KH", "Cambodia"));
            countryDict.Add(new KeyValuePair<string, string>("CM", "Cameroon"));
            countryDict.Add(new KeyValuePair<string, string>("CA", "Canada"));
            countryDict.Add(new KeyValuePair<string, string>("CV", "Cape Verde"));
            countryDict.Add(new KeyValuePair<string, string>("KY", "Cayman Islands"));
            countryDict.Add(new KeyValuePair<string, string>("CF", "Central African Republic"));
            countryDict.Add(new KeyValuePair<string, string>("TD", "Chad"));
            countryDict.Add(new KeyValuePair<string, string>("CL", "Chile"));
            countryDict.Add(new KeyValuePair<string, string>("CN", "China"));
            countryDict.Add(new KeyValuePair<string, string>("CX", "Christmas Island"));
            countryDict.Add(new KeyValuePair<string, string>("CC", "Cocos (Keeling) Islands"));
            countryDict.Add(new KeyValuePair<string, string>("CO", "Colombia"));
            countryDict.Add(new KeyValuePair<string, string>("KM", "Comoros"));
            countryDict.Add(new KeyValuePair<string, string>("CG", "Congo"));
            countryDict.Add(new KeyValuePair<string, string>("CD", "Congo The Democratic Republic of the"));
            countryDict.Add(new KeyValuePair<string, string>("CK", "Cook Islands"));
            countryDict.Add(new KeyValuePair<string, string>("CR", "Costa Rica"));
            countryDict.Add(new KeyValuePair<string, string>("CI", "Côte d'Ivoire"));
            countryDict.Add(new KeyValuePair<string, string>("HR", "Croatia"));
            countryDict.Add(new KeyValuePair<string, string>("CU", "Cuba"));
            countryDict.Add(new KeyValuePair<string, string>("CY", "Cyprus"));
            countryDict.Add(new KeyValuePair<string, string>("CZ", "Czech Republic"));
            countryDict.Add(new KeyValuePair<string, string>("DK", "Denmark"));
            countryDict.Add(new KeyValuePair<string, string>("DJ", "Djibouti"));
            countryDict.Add(new KeyValuePair<string, string>("DM", "Dominica"));
            countryDict.Add(new KeyValuePair<string, string>("DO", "Dominican Republic"));
            countryDict.Add(new KeyValuePair<string, string>("EC", "Ecuador"));
            countryDict.Add(new KeyValuePair<string, string>("EG", "Egypt"));
            countryDict.Add(new KeyValuePair<string, string>("SV", "El Salvador"));
            countryDict.Add(new KeyValuePair<string, string>("GQ", "Equatorial Guinea"));
            countryDict.Add(new KeyValuePair<string, string>("ER", "Eritrea"));
            countryDict.Add(new KeyValuePair<string, string>("EE", "Estonia"));
            countryDict.Add(new KeyValuePair<string, string>("ET", "Ethiopia"));
            countryDict.Add(new KeyValuePair<string, string>("FK", "Falkland Islands (Malvinas)"));
            countryDict.Add(new KeyValuePair<string, string>("FO", "Faroe Islands"));
            countryDict.Add(new KeyValuePair<string, string>("FJ", "Fiji"));
            countryDict.Add(new KeyValuePair<string, string>("FI", "Finland"));
            countryDict.Add(new KeyValuePair<string, string>("FR", "France"));
            countryDict.Add(new KeyValuePair<string, string>("GF", "French Guiana"));
            countryDict.Add(new KeyValuePair<string, string>("PF", "French Polynesia"));
            countryDict.Add(new KeyValuePair<string, string>("TF", "French Southern Territories"));
            countryDict.Add(new KeyValuePair<string, string>("GA", "Gabon"));
            countryDict.Add(new KeyValuePair<string, string>("GM", "Gambia"));
            countryDict.Add(new KeyValuePair<string, string>("GE", "Georgia"));
            countryDict.Add(new KeyValuePair<string, string>("DE", "Germany"));
            countryDict.Add(new KeyValuePair<string, string>("GH", "Ghana"));
            countryDict.Add(new KeyValuePair<string, string>("GI", "Gibraltar"));
            countryDict.Add(new KeyValuePair<string, string>("GR", "Greece"));
            countryDict.Add(new KeyValuePair<string, string>("GL", "Greenland"));
            countryDict.Add(new KeyValuePair<string, string>("GD", "Grenada"));
            countryDict.Add(new KeyValuePair<string, string>("GP", "Guadeloupe"));
            countryDict.Add(new KeyValuePair<string, string>("GU", "Guam"));
            countryDict.Add(new KeyValuePair<string, string>("GT", "Guatemala"));
            countryDict.Add(new KeyValuePair<string, string>("GG", "Guernsey"));
            countryDict.Add(new KeyValuePair<string, string>("GN", "Guinea"));
            countryDict.Add(new KeyValuePair<string, string>("GW", "Guinea-Bissau"));
            countryDict.Add(new KeyValuePair<string, string>("GY", "Guyana"));
            countryDict.Add(new KeyValuePair<string, string>("HT", "Haiti"));
            countryDict.Add(new KeyValuePair<string, string>("HM", "Heard Island and McDonald Islands"));
            countryDict.Add(new KeyValuePair<string, string>("VA", "Holy See (Vatican City State)"));
            countryDict.Add(new KeyValuePair<string, string>("HN", "Honduras"));
            countryDict.Add(new KeyValuePair<string, string>("HK", "Hong Kong"));
            countryDict.Add(new KeyValuePair<string, string>("HU", "Hungary"));
            countryDict.Add(new KeyValuePair<string, string>("IS", "Iceland"));
            countryDict.Add(new KeyValuePair<string, string>("IN", "India"));
            countryDict.Add(new KeyValuePair<string, string>("ID", "Indonesia"));
            countryDict.Add(new KeyValuePair<string, string>("IR", "Iran Islamic Republic of"));
            countryDict.Add(new KeyValuePair<string, string>("IQ", "Iraq"));
            countryDict.Add(new KeyValuePair<string, string>("IE", "Ireland"));
            countryDict.Add(new KeyValuePair<string, string>("IM", "Isle of Man"));
            countryDict.Add(new KeyValuePair<string, string>("IL", "Israel"));
            countryDict.Add(new KeyValuePair<string, string>("IT", "Italy"));
            countryDict.Add(new KeyValuePair<string, string>("JM", "Jamaica"));
            countryDict.Add(new KeyValuePair<string, string>("JP", "Japan"));
            countryDict.Add(new KeyValuePair<string, string>("JE", "Jersey"));
            countryDict.Add(new KeyValuePair<string, string>("JO", "Jordan"));
            countryDict.Add(new KeyValuePair<string, string>("KZ", "Kazakhstan"));
            countryDict.Add(new KeyValuePair<string, string>("KE", "Kenya"));
            countryDict.Add(new KeyValuePair<string, string>("KI", "Kiribati"));
            countryDict.Add(new KeyValuePair<string, string>("KP", "Korea Democratic People's Republic of"));
            countryDict.Add(new KeyValuePair<string, string>("KR", "Korea Republic of"));
            countryDict.Add(new KeyValuePair<string, string>("KW", "Kuwait"));
            countryDict.Add(new KeyValuePair<string, string>("KG", "Kyrgyzstan"));
            countryDict.Add(new KeyValuePair<string, string>("LA", "Lao People's Democratic Republic"));
            countryDict.Add(new KeyValuePair<string, string>("LV", "Latvia"));
            countryDict.Add(new KeyValuePair<string, string>("LB", "Lebanon"));
            countryDict.Add(new KeyValuePair<string, string>("LS", "Lesotho"));
            countryDict.Add(new KeyValuePair<string, string>("LR", "Liberia"));
            countryDict.Add(new KeyValuePair<string, string>("LY", "Libyan Arab Jamahiriya"));
            countryDict.Add(new KeyValuePair<string, string>("LI", "Liechtenstein"));
            countryDict.Add(new KeyValuePair<string, string>("LT", "Lithuania"));
            countryDict.Add(new KeyValuePair<string, string>("LU", "Luxembourg"));
            countryDict.Add(new KeyValuePair<string, string>("MO", "Macao"));
            countryDict.Add(new KeyValuePair<string, string>("MK", "Macedonia The Former Yugoslav Republic of"));
            countryDict.Add(new KeyValuePair<string, string>("MG", "Madagascar"));
            countryDict.Add(new KeyValuePair<string, string>("MW", "Malawi"));
            countryDict.Add(new KeyValuePair<string, string>("MY", "Malaysia"));
            countryDict.Add(new KeyValuePair<string, string>("MV", "Maldives"));
            countryDict.Add(new KeyValuePair<string, string>("ML", "Mali"));
            countryDict.Add(new KeyValuePair<string, string>("MT", "Malta"));
            countryDict.Add(new KeyValuePair<string, string>("MH", "Marshall Islands"));
            countryDict.Add(new KeyValuePair<string, string>("MQ", "Martinique"));
            countryDict.Add(new KeyValuePair<string, string>("MR", "Mauritania"));
            countryDict.Add(new KeyValuePair<string, string>("MU", "Mauritius"));
            countryDict.Add(new KeyValuePair<string, string>("YT", "Mayotte"));
            countryDict.Add(new KeyValuePair<string, string>("MX", "Mexico"));
            countryDict.Add(new KeyValuePair<string, string>("FM", "Micronesia Federated States of"));
            countryDict.Add(new KeyValuePair<string, string>("MD", "Moldova Republic of"));
            countryDict.Add(new KeyValuePair<string, string>("MC", "Monaco"));
            countryDict.Add(new KeyValuePair<string, string>("MN", "Mongolia"));
            countryDict.Add(new KeyValuePair<string, string>("ME", "Montenegro"));
            countryDict.Add(new KeyValuePair<string, string>("MS", "Montserrat"));
            countryDict.Add(new KeyValuePair<string, string>("MA", "Morocco"));
            countryDict.Add(new KeyValuePair<string, string>("MZ", "Mozambique"));
            countryDict.Add(new KeyValuePair<string, string>("MM", "Myanmar"));
            countryDict.Add(new KeyValuePair<string, string>("NA", "Namibia"));
            countryDict.Add(new KeyValuePair<string, string>("NR", "Nauru"));
            countryDict.Add(new KeyValuePair<string, string>("NP", "Nepal"));
            countryDict.Add(new KeyValuePair<string, string>("NL", "Netherlands"));
            countryDict.Add(new KeyValuePair<string, string>("AN", "Netherlands Antilles"));
            countryDict.Add(new KeyValuePair<string, string>("NC", "New Caledonia"));
            countryDict.Add(new KeyValuePair<string, string>("NZ", "New Zealand"));
            countryDict.Add(new KeyValuePair<string, string>("NI", "Nicaragua"));
            countryDict.Add(new KeyValuePair<string, string>("NE", "Niger"));
            countryDict.Add(new KeyValuePair<string, string>("NG", "Nigeria"));
            countryDict.Add(new KeyValuePair<string, string>("NU", "Niue"));
            countryDict.Add(new KeyValuePair<string, string>("NF", "Norfolk Island"));
            countryDict.Add(new KeyValuePair<string, string>("MP", "Northern Mariana Islands"));
            countryDict.Add(new KeyValuePair<string, string>("NO", "Norway"));
            countryDict.Add(new KeyValuePair<string, string>("OM", "Oman"));
            countryDict.Add(new KeyValuePair<string, string>("PK", "Pakistan"));
            countryDict.Add(new KeyValuePair<string, string>("PW", "Palau"));
            countryDict.Add(new KeyValuePair<string, string>("PS", "Palestinian Territory Occupied"));
            countryDict.Add(new KeyValuePair<string, string>("PA", "Panama"));
            countryDict.Add(new KeyValuePair<string, string>("PG", "Papua New Guinea"));
            countryDict.Add(new KeyValuePair<string, string>("PY", "Paraguay"));
            countryDict.Add(new KeyValuePair<string, string>("PE", "Peru"));
            countryDict.Add(new KeyValuePair<string, string>("PH", "Philippines"));
            countryDict.Add(new KeyValuePair<string, string>("PN", "Pitcairn"));
            countryDict.Add(new KeyValuePair<string, string>("PL", "Poland"));
            countryDict.Add(new KeyValuePair<string, string>("PT", "Portugal"));
            countryDict.Add(new KeyValuePair<string, string>("PR", "Puerto Rico"));
            countryDict.Add(new KeyValuePair<string, string>("QA", "Qatar"));
            countryDict.Add(new KeyValuePair<string, string>("RE", "Réunion"));
            countryDict.Add(new KeyValuePair<string, string>("RO", "Romania"));
            countryDict.Add(new KeyValuePair<string, string>("RU", "Russian Federation"));
            countryDict.Add(new KeyValuePair<string, string>("RW", "Rwanda"));
            countryDict.Add(new KeyValuePair<string, string>("BL", "Saint Barthélemy"));
            countryDict.Add(new KeyValuePair<string, string>("SH", "Saint Helena Ascension and Tristan Da Cunha"));
            countryDict.Add(new KeyValuePair<string, string>("KN", "Saint Kitts and Nevis"));
            countryDict.Add(new KeyValuePair<string, string>("LC", "Saint Lucia"));
            countryDict.Add(new KeyValuePair<string, string>("MF", "Saint Martin"));
            countryDict.Add(new KeyValuePair<string, string>("PM", "Saint Pierre and Miquelon"));
            countryDict.Add(new KeyValuePair<string, string>("VC", "Saint Vincent and the Grenadines"));
            countryDict.Add(new KeyValuePair<string, string>("WS", "Samoa"));
            countryDict.Add(new KeyValuePair<string, string>("SM", "San Marino"));
            countryDict.Add(new KeyValuePair<string, string>("ST", "Sao Tome and Principe"));
            countryDict.Add(new KeyValuePair<string, string>("SA", "Saudi Arabia"));
            countryDict.Add(new KeyValuePair<string, string>("SN", "Senegal"));
            countryDict.Add(new KeyValuePair<string, string>("RS", "Serbia"));
            countryDict.Add(new KeyValuePair<string, string>("SC", "Seychelles"));
            countryDict.Add(new KeyValuePair<string, string>("SL", "Sierra Leone"));
            countryDict.Add(new KeyValuePair<string, string>("SG", "Singapore"));
            countryDict.Add(new KeyValuePair<string, string>("SK", "Slovakia"));
            countryDict.Add(new KeyValuePair<string, string>("SI", "Slovenia"));
            countryDict.Add(new KeyValuePair<string, string>("SB", "Solomon Islands"));
            countryDict.Add(new KeyValuePair<string, string>("SO", "Somalia"));
            countryDict.Add(new KeyValuePair<string, string>("ZA", "South Africa"));
            countryDict.Add(new KeyValuePair<string, string>("GS", "South Georgia and the South Sandwich Islands"));
            countryDict.Add(new KeyValuePair<string, string>("ES", "Spain"));
            countryDict.Add(new KeyValuePair<string, string>("LK", "Sri Lanka"));
            countryDict.Add(new KeyValuePair<string, string>("SD", "Sudan"));
            countryDict.Add(new KeyValuePair<string, string>("SR", "Suriname"));
            countryDict.Add(new KeyValuePair<string, string>("SJ", "Svalbard and Jan Mayen"));
            countryDict.Add(new KeyValuePair<string, string>("SZ", "Swaziland"));
            countryDict.Add(new KeyValuePair<string, string>("SE", "Sweden"));
            countryDict.Add(new KeyValuePair<string, string>("CH", "Switzerland"));
            countryDict.Add(new KeyValuePair<string, string>("SY", "Syrian Arab Republic"));
            countryDict.Add(new KeyValuePair<string, string>("TW", "Taiwan Province of China"));
            countryDict.Add(new KeyValuePair<string, string>("TJ", "Tajikistan"));
            countryDict.Add(new KeyValuePair<string, string>("TZ", "Tanzania United Republic of"));
            countryDict.Add(new KeyValuePair<string, string>("TH", "Thailand"));
            countryDict.Add(new KeyValuePair<string, string>("TL", "Timor-Leste"));
            countryDict.Add(new KeyValuePair<string, string>("TG", "Togo"));
            countryDict.Add(new KeyValuePair<string, string>("TK", "Tokelau"));
            countryDict.Add(new KeyValuePair<string, string>("TO", "Tonga"));
            countryDict.Add(new KeyValuePair<string, string>("TT", "Trinidad and Tobago"));
            countryDict.Add(new KeyValuePair<string, string>("TN", "Tunisia"));
            countryDict.Add(new KeyValuePair<string, string>("TR", "Turkey"));
            countryDict.Add(new KeyValuePair<string, string>("TM", "Turkmenistan"));
            countryDict.Add(new KeyValuePair<string, string>("TC", "Turks and Caicos Islands"));
            countryDict.Add(new KeyValuePair<string, string>("TV", "Tuvalu"));
            countryDict.Add(new KeyValuePair<string, string>("UG", "Uganda"));
            countryDict.Add(new KeyValuePair<string, string>("UA", "Ukraine"));
            countryDict.Add(new KeyValuePair<string, string>("AE", "United Arab Emirates"));
            countryDict.Add(new KeyValuePair<string, string>("GB", "United Kingdom"));
            countryDict.Add(new KeyValuePair<string, string>("US", "United States"));
            countryDict.Add(new KeyValuePair<string, string>("UM", "United States Minor Outlying Islands"));
            countryDict.Add(new KeyValuePair<string, string>("UY", "Uruguay"));
            countryDict.Add(new KeyValuePair<string, string>("UZ", "Uzbekistan"));
            countryDict.Add(new KeyValuePair<string, string>("VU", "Vanuatu"));
            countryDict.Add(new KeyValuePair<string, string>("VE", "Venezuela Bolivarian Republic of"));
            countryDict.Add(new KeyValuePair<string, string>("VN", "Viet Nam"));
            countryDict.Add(new KeyValuePair<string, string>("VG", "Virgin Islands British"));
            countryDict.Add(new KeyValuePair<string, string>("VI", "Virgin Islands U.S."));
            countryDict.Add(new KeyValuePair<string, string>("WF", "Wallis and Futuna"));
            countryDict.Add(new KeyValuePair<string, string>("EH", "Western Sahara"));
            countryDict.Add(new KeyValuePair<string, string>("YE", "Yemen"));
            countryDict.Add(new KeyValuePair<string, string>("ZM", "Zambia"));
            countryDict.Add(new KeyValuePair<string, string>("ZW", "Zimbabwe"));
        }

        public IActionResult Index()
        {
            var viewModel = new AdminViewModel();
            viewModel.TotalKYCPending = _sysUserService.Queryable().Count(x => x.KYCVerified.HasValue && !x.KYCVerified.Value);
            viewModel.TotalKYCVerified = _sysUserService.Queryable().Count(x => x.KYCVerified.HasValue && x.KYCVerified.Value);
            viewModel.TotalUser = _sysUserService.Queryable().Count();
            viewModel.TotalNews = _newsService.Queryable().Count();
            return View(viewModel);
        }

        #region User
        public IActionResult AllUser()
        {
            var viewModel = new AllUserViewModel();
            return View(viewModel);
        }

        public IActionResult User(int id)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == id);
            var viewModel = Mapper.Map<UserDashboardAdminViewModel>(user);
            decimal coinRate = CoinExchangeExtension.CoinExchanging();
            var tokenRate = _settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.BTCToTokenRate).Value;
            viewModel.TotalBalance = user.ETHAmount * coinRate + user.TokenAmount / decimal.Parse(tokenRate) + user.BTCAmount;
            viewModel.Country = countryDict[user.Country];
            return View(viewModel);
        }

        public IActionResult EditUser(int id)
        {
            var user = _sysUserService.Queryable()
                .FirstOrDefault(x => x.Id == id);

            return PartialView("_EditUser", Mapper.Map<SysUserViewModel>(user));
        }

        [HttpPost]
        public IActionResult UpdateUser(SysUserViewModel viewModel)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == viewModel.Id);
            if (user != null)
            {
                var existingUser = _sysUserService.Queryable().FirstOrDefault(x => x.Email == viewModel.Email);
                if (existingUser != null && existingUser.Id != viewModel.Id)
                    return new JsonResult(new { success = false, name = "email", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidOrExistingEmail") });

                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.Mobile = viewModel.Mobile;
                user.Email = viewModel.Email;
                if (!string.IsNullOrEmpty(viewModel.Password))
                    user.Password = viewModel.Password.ToBCrypt();
                user.StreetAddress = viewModel.StreetAddress.ToLower();
                user.TwoFactorAuthenticationEnable = viewModel.TwoFactorAuthenticationEnable;

                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "UpdateSuccessfully") });
            }

            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
        }

        [HttpPost]
        public IActionResult DeleteUser(SysUserViewModel viewModel)
        {
            var user = _sysUserService.Queryable()
                .FirstOrDefault(x => x.Id == viewModel.Id);

            if (user != null)
            {
                user.IsDeleted = true;

                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "DeleteSuccessfully") });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
        }

        public JsonResult SearchAllUser(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchAllUserFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        public IList<SysUserViewModel> SearchAllUserFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = _sysUserService.Queryable()
                        .Count();

                totalResultsCount = _sysUserService.Queryable()
                        .Count();

                return _sysUserService.Queryable()
                            .Select(x => Mapper.Map<SysUserViewModel>(x))
                            .OrderBy(sortBy, sortDir)
                            .Skip(skip)
                            .Take(take)
                            .ToList();
            }
            else
            {
                filteredResultsCount = _sysUserService.Queryable()
                        .Where(x => x.FirstName.Contains(searchBy) || x.LastName.Contains(searchBy)
                        || x.Email.Contains(searchBy) || x.StreetAddress.Contains(searchBy) || x.Mobile.Contains(searchBy))
                        .Count();

                totalResultsCount = _sysUserService.Queryable()
                        .Count();

                return _sysUserService.Queryable()
                        .Where(x => x.FirstName.Contains(searchBy) || x.LastName.Contains(searchBy)
                        || x.Email.Contains(searchBy) || x.StreetAddress.Contains(searchBy) || x.Mobile.Contains(searchBy))
                        .Select(x => Mapper.Map<SysUserViewModel>(x))
                        .OrderBy(sortBy, sortDir)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
            }
        }
        #endregion

        #region KYC
        public IActionResult KYCVerify()
        {
            var viewModel = new KYCVerifyViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateKYCVerify(int id)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == id);
            user.KYCVerified = true;

            // Transfer prize
            var lotteryHistorys = _lotteryHistoryService
                              .Query().Include(x => x.LotteryPrize).Select()
                              .Where(x => x.SysUserId == user.Id && x.Result == EnumGameResult.KYC_PENDING.ToString())
                              .ToList();
            foreach (var lotteryHistory in lotteryHistorys)
            {
                user.TokenAmount += lotteryHistory.LotteryPrize.Value;
                // Update status
                lotteryHistory.Result = EnumGameResult.WIN.ToString();
                _lotteryHistoryService.Update(lotteryHistory);
            }

            // Save DB
            _sysUserService.Update(user);
            _unitOfWork.SaveChanges();

            // Send email
            var template = _templateService.Queryable().FirstOrDefault(x => x.Name == EnumTemplate.KYCVerify.ToString());
            var kycVerifyEmailTemplateViewModel = Mapper.Map<KYCVerifyEmailTemplateViewModel>(user);

            template.Body = _viewRenderService.RenderToStringAsync("/Views/Admin/_KYCVerifyEmailTemplate.cshtml", kycVerifyEmailTemplateViewModel).Result;
            EmailHelper.Send(Mapper.Map<TemplateViewModel>(template), user.Email);

            return new JsonResult(new { success = true, message = user.FirstName + $" {LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "KYCVerifiedEmailSent")}" });
        }

        [HttpPost]
        public IActionResult CancelKYCVerify(int id)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == id);
            user.KYCVerified = null;
            user.KYCCreatedDate = null;
            user.FrontSide = null;
            user.BackSide = null;

            _sysUserService.Update(user);
            _unitOfWork.SaveChanges();

            return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "CancelSuccessfully") });
        }

        public JsonResult SearchKYCVerify(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchKYCVerifyFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        public IList<SysUserViewModel> SearchKYCVerifyFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = _sysUserService.Queryable()
                        .Where(x => x.KYCVerified.HasValue)
                        .Count();

                totalResultsCount = _sysUserService.Queryable()
                        .Where(x => x.KYCVerified.HasValue)
                        .Count();

                return _sysUserService.Queryable()
                            .Where(x => x.KYCVerified.HasValue)
                            .OrderBy("KYCCreatedDate", false)
                            .Select(x => Mapper.Map<SysUserViewModel>(x))
                            .OrderBy(sortBy, sortDir)
                            .Skip(skip)
                            .Take(take)
                            .ToList();
            }
            else
            {
                filteredResultsCount = _sysUserService.Queryable()
                        .Where(x => x.KYCVerified.HasValue)
                        .Where(x => x.FirstName.Contains(searchBy) || x.LastName.Contains(searchBy)
                        || x.Email.Contains(searchBy))
                        .Count();

                totalResultsCount = _sysUserService.Queryable()
                        .Where(x => x.KYCVerified.HasValue)
                        .Count();

                return _sysUserService.Queryable()
                        .Where(x => x.KYCVerified.HasValue)
                        .Where(x => x.FirstName.Contains(searchBy) || x.LastName.Contains(searchBy)
                        || x.Email.Contains(searchBy))
                        .Select(x => Mapper.Map<SysUserViewModel>(x))
                        .OrderBy(sortBy, sortDir)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
            }
        }
        #endregion

        #region News
        public IActionResult News()
        {
            var viewModel = new NewsViewModel();
            return View(viewModel);
        }

        public IActionResult EditNews(int id)
        {
            var news = _newsService.Queryable()
                .FirstOrDefault(x => x.Id == id);

            return PartialView("_EditNews", Mapper.Map<NewsViewModel>(news));
        }

        public JsonResult SearchNews(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchNewsFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        public IList<NewsViewModel> SearchNewsFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = _newsService.Queryable()
                        .Count();

                totalResultsCount = _newsService.Queryable()
                        .Count();

                return _newsService.Queryable()
                            .Select(x => Mapper.Map<NewsViewModel>(x))
                            .OrderBy(sortBy, sortDir)
                            .Skip(skip)
                            .Take(take)
                            .ToList();
            }
            else
            {
                filteredResultsCount = _newsService.Queryable()
                        .Where(x => x.Title.Contains(searchBy) || x.ShortDescription.Contains(searchBy))
                        .Count();

                totalResultsCount = _newsService.Queryable()
                        .Count();

                return _newsService.Queryable()
                        .Where(x => x.Title.Contains(searchBy) || x.ShortDescription.Contains(searchBy))
                        .Select(x => Mapper.Map<NewsViewModel>(x))
                        .OrderBy(sortBy, sortDir)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
            }
        }
        #endregion
    }
}