using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using LumenWorks.Framework.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using LumenWorks.Framework.IO.Csv;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class MepBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);

        public MEP GetById(int idMep)
        {
            return _ctx.MEP.Find(idMep);
        }

        public MEP GetBySn(string sn)
        {
            return _ctx.MEP.FirstOrDefault(mep => mep.SerialNumber.Equals(sn));
        }
        public MEPCustom GetBySnCustom(string sn)
        {
            var mep = _ctx.MEP.FirstOrDefault(mepdata => mepdata.SerialNumber.Equals(sn));
            var mepcustom = new MEPCustom() { Area = mep.Area,
                ArrNumber = mep.ArrNumber,
                Comment = mep.Comment,
                CustId = mep.CustId,
                CustomerName = mep.CustomerName,
                DefinitionIndustry = mep.DefinitionIndustry,
                DefinitionSubIndustry = mep.DefinitionSubIndustry,
                DeliveryDate = mep.DeliveryDate.HasValue ?  mep.DeliveryDate.Value.ToString("yyyy-MM-dd") : "-",
                EngineMake = mep.EngineMake,
                EngineModel = mep.EngineModel,
                EngineSerialNumber = mep.EngineSerialNumber,
                ENGMACLTR = mep.ENGMACLTR,
                ENGMACLTRDescription = mep.ENGMACLTRDescription,
                EquipmentNumber = mep.EquipmentNumber,
                EquipmentStatus = mep.EquipmentStatus,
                GroupName = mep.GroupName,
                IdMEP = mep.IdMEP,
                IndCode = mep.IndCode,
                LifeToDateSMU = mep.LifeToDateSMU,
                LocationDetail = mep.LocationDetail,
                Make = mep.Make,
                Model = mep.Model,
                Plant = mep.Plant,
                PT = mep.PT,
                PTDescription = mep.PTDescription,
                PurchaseDate = mep.PurchaseDate.HasValue ? mep.PurchaseDate.Value.ToString("yyyy-MM-dd") : "-",
                PWCCode = mep.PWCCode,
                Qty = mep.Qty,
                Region = mep.Region,
                RepsName = mep.RepsName,
                SalesOffice = mep.SalesOffice,
                SalesOfficeDescription = mep.SalesOfficeDescription,
                SerialNumber = mep.SerialNumber,
                ShipToAddress = mep.ShipToAddress,
                ShipToID = mep.ShipToID,
                SMU = mep.SMU,
                SMUUpDate = mep.SMUUpDate.HasValue ?  mep.SMUUpDate.Value.ToString("yyyy-MM-dd") : "-" 
            };
            return mepcustom;
        }
        public List<string> GetSuggestion(string sn)
        {
            List<string> suggestion = new List<string>();
            var listMep = _ctx.MEP.Where(q => q.SerialNumber.Contains(sn));

            foreach(var itemMep in listMep)
            {
                suggestion.Add(itemMep.SerialNumber);
            }

            return suggestion;
        }

        public void UpdateMake()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                {"AI", "ACCESS INNOVATIONS PTY LTD"},
                {"AH", "AKERMAN-HW"},
                {"AL", "ALBARET."},
                {"AG", "ALLIGHT"},
                {"AC", "ALLIS CHALMERS."},
                {"AR", "AMERICAN HOIST"},
                {"AM", "AMMANN"},
                {"AT", "ATLAS COPCO"},
                {"AB", "AVELING BARFORD."},
                {"BD", "BADGER"},
                {"BA", "BALDERSON."},
                {"BG", "BARBER GREEHE"},
                {"BF", "BEDFORD."},
                {"BL", "BELSHINA TIRES"},
                {"BM", "BEML."},
                {"BN", "BENATI"},
                {"BU", "BEUTHLING"},
                {"BT", "BITELLI"},
                {"BK", "BLAW-KNOX"},
                {"BO", "BOMAG."},
                {"BS", "BRIDGESTONE"},
                {"BR", "BROS"},
                {"BE", "BUCYRUS"},
                {"CM", "C.M.I."},
                {"AA", "CATERPILLAR"},
                {"CT", "CEDARAPIDS"},
                {"CH", "CHAMPION."},
                {"CE", "CLARK EQUIPMENT."},
                {"CL", "CLINE TRUCK MFG."},
                {"CU", "CUMMINS."},
                {"CO", "CUSTOMER ORDER"},
                {"CW", "CWS INDUSTRIES (MFG) CORP."},
                {"DW", "DAEWOO"},
                {"DG", "DAIHATSU"},
                {"DA", "DART"},
                {"DB", "DAUBERT"},
                {"DL", "DELTA"},
                {"DN", "DENYO."},
                {"DF", "DERRUPPE"},
                {"DI", "DETROIT"},
                {"DU", "DEUTZ."},
                {"DZ", "DEUTZ-ALLIS"},
                {"DX", "DIX"},
                {"DJ", "DJB."},
                {"DD", "DODICH"},
                {"DO", "DONG FENG."},
                {"AN", "DOOSAN"},
                {"DS", "DRESSER"},
                {"DE", "DRESSTA"},
                {"DT", "DRILTECH"},
                {"DR", "DROTT"},
                {"DM", "DYMAX INC"},
                {"DH", "DYNAHOE"},
                {"DC", "DYNAMIC"},
                {"DY", "DYNAPAC."},
                {"EW", "E.W.K."},
                {"EA", "EATON, ETN"},
                {"EC", "ECOMAT"},
                {"EE", "EDER"},
                {"EL", "ELPHINSTONE"},
                {"EM", "EMD/GM"},
                {"EN", "ENERCON"},
                {"EP", "ENGINE PROJECT"},
                {"ER", "ERLAU"},
                {"ES", "ESCO"},
                {"ET", "ETNYRE"},
                {"ED", "EUCLID DIVISION"},
                {"EU", "EURO TIRE"},
                {"FD", "F.D.I."},
                {"WF", "F.G. WILSON"},
                {"FF", "FAI"},
                {"FM", "FAIRBANKS MORSE"},
                {"FP", "FAUN-FRISCH"},
                {"FY", "FEDERAL MOGUL"},
                {"FA", "FIAT-ALLIS."},
                {"FT", "FIGGIE INTER"},
                {"FE", "FIGGIE/ESSIC"},
                {"FJ", "FINLAY"},
                {"FI", "FINNINGS"},
                {"FS", "FIRE & SAFETY INDUSTRIES"},
                {"FX", "FLEXOR"},
                {"FC", "FMC CORPORATION"},
                {"FO", "FORD"},
                {"FN", "FORD NEW HOL"},
                {"FV", "FORD-VERSATI"},
                {"FL", "FORTRESS ALL"},
                {"FR", "FRAM"},
                {"FK", "FRANKLIN"},
                {"FU", "FURUKAWA."},
                {"FB", "FUTURE FAB"},
                {"FW", "FWD CORP."},
                {"GA", "GALLION"},
                {"GT", "GATES"},
                {"GC", "GEN ELECTRIC"},
                {"GN", "GENERAC"},
                {"GR", "GENERAC CORP."},
                {"GM", "GENERAL MOTOR."},
                {"GE", "GENIE"},
                {"GL", "GERLINGER"},
                {"GH", "GHINASSI (CGR)"},
                {"GI", "GIMAC"},
                {"GK", "GKN"},
                {"GW", "GODWIN"},
                {"GO", "GOMACO"},
                {"GY", "GOODYEAR TIRES"},
                {"GD", "GRADALL"},
                {"GF", "GROUND FORCE MFG., LLC"},
                {"GV", "GROVE"},
                {"GS", "GS"},
                {"GU", "GURIA"},
                {"HW", "HAIN-WERNER"},
                {"HL", "HALLA."},
                {"HA", "HAMM AG"},
                {"HC", "HANDLING COST"},
                {"HG", "HANOMAG"},
                {"HV", "HARTECH"},
                {"HU", "HAULAMATIC"},
                {"HH", "HAULMAX"},
                {"HP", "HAULMAX TRUCK"},
                {"HK", "HAYASAKI"},
                {"HZ", "HAYAZAKI"},
                {"HF", "HEATHFIELD"},
                {"HE", "HENDERSON"},
                {"HN", "HINDEX COY."},
                {"HD", "HINDUSTAN"},
                {"HT", "HINO TRUCK."},
                {"HO", "HINOMOTO"},
                {"HI", "HITACHI."},
                {"HB", "HOKUETSU"},
                {"HR", "HUBER CORP"},
                {"HS", "HUSKY"},
                {"HX", "HYDREX"},
                {"HQ", "HYDRO AX"},
                {"HJ", "HYDROMAC"},
                {"HM", "HY-MAC"},
                {"HY", "HYSTER"},
                {"IP", "I.P.D."},
                {"IC", "ICON"},
                {"IG", "IGLAND"},
                {"II", "IHI."},
                {"IN", "INDIAN G"},
                {"IR", "INGERSOLL-RAND"},
                {"IM", "INGRAM"},
                {"IY", "INSLEY"},
                {"IH", "INTERNATIONAL HARVESTER/IHC."},
                {"IO", "IOWA MOLD TOOLING CO; INC"},
                {"IS", "ISHIKAWAJIMA"},
                {"IK", "ISHIKO."},
                {"ZT", "ISUZU TRUCK."},
                {"IZ", "ISUZU."},
                {"IA", "ITAL"},
                {"IT", "ITRAC"},
                {"IV", "IVECO"},
                {"IW", "IWATEFUJI"},
                {"IU", "IZUMI"},
                {"JC", "J.C. BAMFORD"},
                {"JA", "JAMES"},
                {"JS", "JAPAN STEELWORKS"},
                {"JB", "JEC BODY"},
                {"JE", "JEC TYRE HANDLER"},
                {"JI", "JIDECO"},
                {"JL", "JLG"},
                {"JD", "JOHN DEERE."},
                {"KA", "KAELBLE"},
                {"KN", "KALDNES FORKLIFT TRUCKS"},
                {"KT", "KATO."},
                {"KC", "KC"},
                {"KW", "KENWORTH."},
                {"KF", "KERFAB"},
                {"KI", "KIMCO."},
                {"KO", "KOBE (KOBELCO)."},
                {"KG", "KOEHRING."},
                {"KL", "KOHLER"},
                {"KM", "KOMATSU."},
                {"KJ", "KONNE-JYA-OY"},
                {"KY", "KOOTENAY MFG"},
                {"KR", "KORI."},
                {"KD", "KORODY."},
                {"KE", "KRAMER"},
                {"KS", "KS"},
                {"KB", "KUBOTA."},
                {"LA", "LALTESI"},
                {"LD", "LANDINI"},
                {"LE", "LANNEN"},
                {"LL", "LATIL"},
                {"LT", "LAYTON"},
                {"LR", "LEBRERO"},
                {"LY", "LEEBOY"},
                {"LG", "LEGRA"},
                {"LV", "LEVERTON MATERIALS HANDLING"},
                {"LB", "LIBRA"},
                {"LH", "LIEBHERR"},
                {"LI", "LINCOLN"},
                {"LK", "LINK-BELT"},
                {"LO", "LOKOMO"},
                {"LN", "LORAIN"},
                {"LU", "LUCAS"},
                {"MC", "MACK TRUCK."},
                {"MK", "MAK"},
                {"MO", "MANITOU"},
                {"MN", "MARATHON LET"},
                {"MF", "MASSEY FERGUSON."},
                {"MU", "MBU"},
                {"MG", "MEGA WATER TANKER"},
                {"MB", "MERCEDES BENZ."},
                {"ME", "MICHELIN TYRE"},
                {"MI", "MICHIGAN."},
                {"MD", "MIDLAND MACHINERY"},
                {"MA", "MIKASA"},
                {"ML", "MILLER"},
                {"MT", "MITSUBISHI"},
                {"MS", "MITSUI"},
                {"MV", "MODRA"},
                {"MR", "M-R-S"},
                {"MP", "MULTIFLO PUMP"},
                {"MW", "MWM."},
                {"NC", "NAKAMACHI"},
                {"NJ", "NANGJING TURBIN"},
                {"NR", "NATRA RAYA"},
                {"NA", "NAVISTAR INT"},
                {"NL", "NEAL"},
                {"NE", "NELSON INDUSTRIES"},
                {"NF", "NF"},
                {"NI", "NIIGATA"},
                {"NK", "NIKKO (JSW)."},
                {"NS", "NIPPON-SHARY"},
                {"NT", "NISSAN TRUCK."},
                {"NN", "NISSAN."},
                {"NV", "NORDVERK"},
                {"NO", "NORICUM"},
                {"NW", "NORTHWEST MFG"},
                {"NP", "NPK"},
                {"OK", "O&K"},
                {"ON", "ONAN"},
                {"OR", "ORDERNARY"},
                {"OA", "OSAKA"},
                {"ZZ", "OTHER"},
                {"LM", "OTHER LOCAL MANUFACTURER"},
                {"OS", "OTSUKA."},
                {"OW", "OWATONNA"},
                {"PQ", "P&H"},
                {"PA", "PACCAR"},
                {"PT", "PACIFIC TRUCK*"},
                {"PZ", "PALAZZANI"},
                {"PD", "PAPANINDO NUGRAKARYA"},
                {"PP", "PARKER PLANT"},
                {"PU", "PAUS"},
                {"PJ", "PEGSON"},
                {"PF", "PERFECT"},
                {"PE", "PERKINS."},
                {"PL", "PERLINI"},
                {"PB", "PETTIBONE"},
                {"PH", "PHILLIPS"},
                {"PS", "PIELSTIK/NGT"},
                {"PG", "PINGON"},
                {"PN", "PIONEER"},
                {"PY", "PLYMOUTH"},
                {"PI", "PMI"},
                {"PC", "POCLAIN."},
                {"PO", "POTAIN."},
                {"PR", "PRENTICE"},
                {"PX", "PRIESTMAN"},
                {"PV", "PRIME MOVER"},
                {"PM", "PRIMIERA"},
                {"TU", "PTTU"},
                {"PK", "PUCKETT"},
                {"RD", "RABAUD"},
                {"RX", "RAMMAX"},
                {"RL", "RANDELLS"},
                {"RY", "RAYGO/BROS"},
                {"RN", "RENAULT"},
                {"RZ", "RENTAL NO NI"},
                {"RQ", "REX"},
                {"RE", "RICHIER"},
                {"RP", "RIMPULL"},
                {"RT", "ROADLESS TRA"},
                {"RA", "ROADTEC"},
                {"RK", "ROCK"},
                {"RG", "ROGERS TRAILER"},
                {"RR", "ROLLS ROYCE"},
                {"RI", "ROME INDUSTRY"},
                {"RV", "ROMERO"},
                {"RC", "ROSCO MFG."},
                {"RS", "ROSSI"},
                {"RO", "ROYAL TRACTOR"},
                {"RM", "RUMANIAN"},
                {"RU", "RUSSIA"},
                {"RB", "RUSTON-BUCY"},
                {"SJ", "S.A.M.E."},
                {"SK", "SACM/UNI"},
                {"SA", "SAKAI."},
                {"SQ", "SALSCO"},
                {"SZ", "SAMBRON"},
                {"SM", "SAMSUNG"},
                {"SB", "SANGGAR SARANA BAJA"},
                {"SD", "SANTAL"},
                {"SS", "SCANIA/SAAB"},
                {"SH", "SCHAEFF"},
                {"SF", "SCHOPF"},
                {"SW", "SCHWING"},
                {"SE", "SENNEBOGEN"},
                {"AU", "SHANTUI"},
                {"SC", "SHERPA CRANE."},
                {"WY", "SHINDAIWA"},
                {"SI", "SICOM"},
                {"SN", "SINAC"},
                {"SO", "SOUTHWEST HU"},
                {"SY", "SPERRY-NH"},
                {"SG", "STAMFORD"},
                {"SV", "STAVOSTROJ"},
                {"ST", "STEWART"},
                {"SR", "STEYR"},
                {"SX", "STROJEXPORT"},
                {"SL", "SULLAIR"},
                {"SU", "SUMITOMO"},
                {"SP", "SYKES PUMP"},
                {"TA", "TAKEUCHI"},
                {"TP", "TAMPO"},
                {"TL", "TAYLOR"},
                {"TY", "TCI"},
                {"TC", "TCM."},
                {"TT", "TEMMA TERRA"},
                {"TB", "TERBERG"},
                {"TX", "TEREX"},
                {"TS", "TESCO"},
                {"TI", "TIGER"},
                {"TG", "TIGER ENGINEERING"},
                {"TM", "TIM KEN"},
                {"TJ", "TIMBER JACK."},
                {"TK", "TOMEN KENKI"},
                {"TH", "TOYOSHA"},
                {"TO", "TOYOTA"},
                {"TR", "TRAILKING"},
                {"TF", "TREE FARMER"},
                {"TN", "TROJAN"},
                {"TW", "TRW"},
                {"TD", "TWIN DISC"},
                {"UN", "UNF"},
                {"UI", "UNION"},
                {"UR", "UNIT RIG"},
                {"UG", "UNIVERSAL GR"},
                {"U", "UNKNOWN"},
                {"UO", "UNOCO"},
                {"UP", "URSUS-PERONI"},
                {"UE", "USED EQUIPMENT"},
                {"US", "USG"},
                {"VA", "VALMET"},
                {"VS", "VAMMAS"},
                {"VI", "VENIERI"},
                {"VE", "VERMEER"},
                {"VP", "VIBROPLUS"},
                {"VH", "VIELHABEN"},
                {"VM", "VME"},
                {"VO", "VOLVO."},
                {"VR", "VRETEN"},
                {"WA", "WABCO."},
                {"WC", "WACKER"},
                {"WN", "WAGNER"},
                {"WD", "WALDRON"},
                {"WE", "WARNER/SWASEY"},
                {"WS", "WARTSILA"},
                {"WV", "WARTSILA VASA."},
                {"WO", "WATCO"},
                {"WK", "WAUKESHA"},
                {"WB", "WEBER"},
                {"WM", "WEHRBAHM"},
                {"WZ", "WEILER"},
                {"WL", "WELTE"},
                {"WQ", "WESTERN STAR TRUCK"},
                {"WT", "WHITE"},
                {"WH", "WHITE/HERCLS"},
                {"WI", "WILL FIT"},
                {"WR", "WILMAR"},
                {"WG", "WIRTGEN"},
                {"WX", "WIX"},
                {"WW", "WOODWARD"},
                {"VV", "XXXXXXX"},
                {"YA", "YALE & TOWNE."},
                {"YM", "YANMAR"},
                {"YO", "YOUNG"},
                {"YS", "YUGOSLAV"},
                {"YB", "YUMBO"},
                {"YU", "YUTANI."},
                {"ZP", "ZEPPELIN"},
                {"ZS", "ZEPPLIN/SCHA"},
                {"ZM", "ZETTELMEYER"},
                {"ZF", "ZF MARINE GEAR BOX"},
                {"MM", "SEM"}
            };


            foreach (var item in dict)
            {
                using (TsicsContext db = new TsicsContext())
                {
                    db.Database.ExecuteSqlCommand("UPDATE MEP SET Make = {0} WHERE Make = {1}", item.Value, item.Key);
                }
            }
        }

        public void BulkUpdateMep(DataTable dataTable, string mainConnectionString)
        {
            if(dataTable.Rows.Count != 0)
            {
                TruncateMep();

                string connection = mainConnectionString;
                    
                SqlConnection sqlConnection = new SqlConnection(connection);

                SqlBulkCopy bulkObject = new SqlBulkCopy(sqlConnection) {DestinationTableName = "MEP"};


                bulkObject.ColumnMappings.Add("Area", "Area");
                bulkObject.ColumnMappings.Add("Region", "Region");
                bulkObject.ColumnMappings.Add("Plant", "Plant");
                bulkObject.ColumnMappings.Add("SalesOffice", "SalesOffice");
                bulkObject.ColumnMappings.Add("SalesOfficeDescription", "SalesOfficeDescription");
                bulkObject.ColumnMappings.Add("CustId", "CustId");
                bulkObject.ColumnMappings.Add("CustomerName", "CustomerName");
                bulkObject.ColumnMappings.Add("ShipToID", "ShipToID");
                bulkObject.ColumnMappings.Add("GroupName", "GroupName");
                bulkObject.ColumnMappings.Add("RepsName", "RepsName");
                bulkObject.ColumnMappings.Add("IndCode", "IndCode");
                bulkObject.ColumnMappings.Add("DefinitionIndustry", "DefinitionIndustry");
                bulkObject.ColumnMappings.Add("DefinitionSubIndustry", "DefinitionSubIndustry");
                bulkObject.ColumnMappings.Add("SerialNumber", "SerialNumber");
                bulkObject.ColumnMappings.Add("ENGMACLTR", "ENGMACLTR");
                bulkObject.ColumnMappings.Add("ENGMACLTRDescription", "ENGMACLTRDescription");
                bulkObject.ColumnMappings.Add("Make", "Make");
                bulkObject.ColumnMappings.Add("PT", "PT");
                bulkObject.ColumnMappings.Add("PTDescription", "PTDescription");
                bulkObject.ColumnMappings.Add("Model", "Model");
                bulkObject.ColumnMappings.Add("Qty", "Qty");
                bulkObject.ColumnMappings.Add("ArrNumber", "ArrNumber");
                bulkObject.ColumnMappings.Add("EquipmentNumber", "EquipmentNumber");
                bulkObject.ColumnMappings.Add("EngineMake", "EngineMake");
                bulkObject.ColumnMappings.Add("EngineModel", "EngineModel");
                bulkObject.ColumnMappings.Add("EngineSerialNumber", "EngineSerialNumber");
                bulkObject.ColumnMappings.Add("PWCCode", "PWCCode");
                bulkObject.ColumnMappings.Add("SMU", "SMU");
                bulkObject.ColumnMappings.Add("LifeToDateSMU", "LifeToDateSMU");
                bulkObject.ColumnMappings.Add("SMUUpDate", "SMUUpDate");
                bulkObject.ColumnMappings.Add("PurchaseDate", "PurchaseDate");
                bulkObject.ColumnMappings.Add("DeliveryDate", "DeliveryDate");
                bulkObject.ColumnMappings.Add("ShipToAddress", "ShipToAddress");
                bulkObject.ColumnMappings.Add("LocationDetail", "LocationDetail");
                bulkObject.ColumnMappings.Add("Comment", "Comment");
                bulkObject.ColumnMappings.Add("EquipmentStatus", "EquipmentStatus");

                sqlConnection.Open();
                bulkObject.BatchSize = 10000;
                bulkObject.BulkCopyTimeout = 0;
                bulkObject.WriteToServer(dataTable);
                sqlConnection.Close();
            }
        }

        private void TruncateMep()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE MEP");

            }
        }

        public DataTable CreateDataTable(string oleDbConnectionString)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("Area", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Region", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Plant", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SalesOffice", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SalesOfficeDescription", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CustId", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ShipToID", typeof(string)));
            dataTable.Columns.Add(new DataColumn("GroupName", typeof(string)));
            dataTable.Columns.Add(new DataColumn("RepsName", typeof(string)));
            dataTable.Columns.Add(new DataColumn("IndCode", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DefinitionIndustry", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DefinitionSubIndustry", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SerialNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ENGMACLTR", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ENGMACLTRDescription", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Make", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PT", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PTDescription", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Model", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Qty", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ArrNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("EquipmentNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("EngineMake", typeof(string)));
            dataTable.Columns.Add(new DataColumn("EngineModel", typeof(string)));
            dataTable.Columns.Add(new DataColumn("EngineSerialNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PWCCode", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SMU", typeof(string)));
            dataTable.Columns.Add(new DataColumn("LifeToDateSMU", typeof(string)));

            dataTable.Columns.Add(new DataColumn("SMUUpDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("PurchaseDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("DeliveryDate", typeof(DateTime)));

            dataTable.Columns.Add(new DataColumn("ShipToAddress", typeof(string)));
            dataTable.Columns.Add(new DataColumn("LocationDetail", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Comment", typeof(string)));
            dataTable.Columns.Add(new DataColumn("EquipmentStatus", typeof(string)));

            using (OleDbConnection conn = new OleDbConnection(oleDbConnectionString))
            {
                conn.Open();
                OleDbCommand command = new OleDbCommand("Select * from [Sheet1$]", conn);
                OleDbDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        dataRow["Area"] = reader["Area"].ToString();
                        dataRow["Region"] = reader["Region"].ToString();
                        dataRow["Plant"] = reader["Plant"].ToString();
                        dataRow["SalesOffice"] = reader["SalesOffice"].ToString();
                        dataRow["SalesOfficeDescription"] = reader["SalesOfficeDescription"].ToString();
                        dataRow["CustId"] = reader["CustId"].ToString();
                        dataRow["CustomerName"] = reader["CustomerName"].ToString();
                        dataRow["ShipToID"] = reader["ShipToID"].ToString();
                        dataRow["GroupName"] = reader["GroupName"].ToString();
                        dataRow["RepsName"] = reader["RepsName"].ToString();
                        dataRow["IndCode"] = reader["IndCode"].ToString();
                        dataRow["DefinitionIndustry"] = reader["DefinitionIndustry"].ToString();
                        dataRow["DefinitionSubIndustry"] = reader["DefinitionSubIndustry"].ToString();
                        dataRow["SerialNumber"] = reader["SerialNumber"].ToString();
                        dataRow["ENGMACLTR"] = reader["ENGMACLTR"].ToString();
                        dataRow["ENGMACLTRDescription"] = reader["ENGMACLTRDescription"].ToString();
                        dataRow["Make"] = reader["Make"].ToString();
                        dataRow["PT"] = reader["PT"].ToString();
                        dataRow["PTDescription"] = reader["PTDescription"].ToString();
                        dataRow["Model"] = reader["Model"].ToString();
                        dataRow["Qty"] = reader["Qty"].ToString();
                        dataRow["ArrNumber"] = reader["ArrNumber"].ToString();
                        dataRow["EquipmentNumber"] = reader["EquipmentNumber"].ToString();
                        dataRow["EngineMake"] = reader["EngineMake"].ToString();
                        dataRow["EngineModel"] = reader["EngineModel"].ToString();
                        dataRow["EngineSerialNumber"] = reader["EngineSerialNumber"].ToString();
                        dataRow["PWCCode"] = reader["PWCCode"].ToString();
                        dataRow["SMU"] = reader["SMU"].ToString();
                        dataRow["LifeToDateSMU"] = reader["LifeToDateSMU"].ToString();

                        if(reader["SMUUpDate"].ToString() != "")
                        {
                            dataRow["SMUUpDate"] = reader["SMUUpDate"].ToString();
                        }
                        
                        if(reader["PurchaseDate"].ToString() != "")
                        {
                            dataRow["PurchaseDate"] = reader["PurchaseDate"].ToString();
                        }
                        
                        if(reader["DeliveryDate"].ToString() != "")
                        {
                            dataRow["DeliveryDate"] = reader["DeliveryDate"].ToString();
                        }

                        dataRow["ShipToAddress"] = reader["ShipToAddress"].ToString();
                        dataRow["LocationDetail"] = reader["LocationDetail"].ToString();
                        dataRow["Comment"] = reader["Comment"].ToString();
                        dataRow["EquipmentStatus"] = reader["EquipmentStatus"].ToString();

                        dataTable.Rows.Add(dataRow);
                    }
                }
            }

            return dataTable;
        }


        public DataTable CreateDataTablePIS(string path)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("SerialNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Model", typeof(string)));
            dataTable.Columns.Add(new DataColumn("AssignDealer", typeof(string)));
            dataTable.Columns.Add(new DataColumn("RepairingDealer", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CatCustomer", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ProgramNo", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Description", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CatPslStatus", typeof(string)));
            dataTable.Columns.Add(new DataColumn("LetterDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("TerminationDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("RepairDealer", typeof(string)));
            dataTable.Columns.Add(new DataColumn("RepairDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("Failure", typeof(string)));
            dataTable.Columns.Add(new DataColumn("LaborHours", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("CatDeliveryDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("PslType", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ServiceClaimAllowanceGroup", typeof(int)));
            dataTable.Columns.Add(new DataColumn("ProductSmuAgeRange1", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CatPartRange1", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CatLaborRange1", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DealerPartRange1", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DealerLaborRange1", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CustomerPartRange1", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CustomerLaborRange1", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ProductSmuAgeRange2", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CatPartRange2", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CatLaborRange2", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DealerPartRange2", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DealerLaborRange2", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CustomerPartRange2", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CustomerLaborRange2", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ProductSmuAgeRange3", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CatPartRange3", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CatLaborRange3", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DealerPartRange3", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DealerLaborRange3", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CustomerPartRange3", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CustomerLaborRange3", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ProductSmuAgeRange4", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CatPartRange4", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CatLaborRange4", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DealerPartRange4", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DealerLaborRange4", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CustomerPartRange4", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CustomerLaborRange4", typeof(string)));

            var strcol = new string[] {
                "SerialNumber",
                "Model",
                "AssignDealer",
                "RepairingDealer",
                "CatCustomer",
                "ProgramNo",
                "Description",
                "CatPslStatus",
                "LetterDate",
                "TerminationDate",
                "RepairDealer",
                "RepairDate",
                "Failure",
                "LaborHours",
                "CatDeliveryDate",
                "PslType",
                "ServiceClaimAllowanceGroup",
                "ProductSmuAgeRange1",
                "CatPartRange1",
                "CatLaborRange1",
                "DealerPartRange1",
                "DealerLaborRange1",
                "CustomerPartRange1",
                "CustomerLaborRange1",
                "ProductSmuAgeRange2",
                "CatPartRange2",
                "CatLaborRange2",
                "DealerPartRange2",
                "DealerLaborRange2",
                "CustomerPartRange2",
                "CustomerLaborRange2",
                "ProductSmuAgeRange3",
                "CatPartRange3",
                "CatLaborRange3",
                "DealerPartRange3",
                "DealerLaborRange3",
                "CustomerPartRange3",
                "CustomerLaborRange3",
                "ProductSmuAgeRange4",
                "CatPartRange4",
                "CatLaborRange4",
                "DealerPartRange4",
                "DealerLaborRange4",
                "CustomerPartRange4",
                "CustomerLaborRange4",
            };


            using (CsvReader csv = new CsvReader(new StreamReader(System.IO.File.OpenRead(path)), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {

                    DataRow dataRow = dataTable.NewRow();
                    for (var i = 0; i < headers.Count(); i++)
                    {
                        if (i == 8)
                        {
                            if (csv[i] != "" )
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if (i == 9)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if (i == 11)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if (i == 14)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else
                        {
                            dataRow[strcol[i]] = csv[i].ToString();
                        }

                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private void TruncatePIS()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE PIS");

            }
        }

        public void SendToXalcPis()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("exec dbo.DPS_CALC_PIS");

            }
        }

        public void BulkUpdatePIS(DataTable dataTable, string mainConnectionString)
        {
            if (dataTable.Rows.Count != 0)
            {
                TruncatePIS();

                string connection = mainConnectionString;

                SqlConnection sqlConnection = new SqlConnection(connection);

                SqlBulkCopy bulkObject = new SqlBulkCopy(sqlConnection) { DestinationTableName = "PIS" };

                bulkObject.ColumnMappings.Add("SerialNumber", "SerialNumber");
                bulkObject.ColumnMappings.Add("Model", "Model");
                bulkObject.ColumnMappings.Add("AssignDealer", "AssignDealer");
                bulkObject.ColumnMappings.Add("RepairingDealer", "RepairingDealer");
                bulkObject.ColumnMappings.Add("CatCustomer", "CatCustomer");
                bulkObject.ColumnMappings.Add("ProgramNo", "ProgramNo");
                bulkObject.ColumnMappings.Add("Description", "Description");
                bulkObject.ColumnMappings.Add("CatPslStatus", "CatPslStatus");
                bulkObject.ColumnMappings.Add("PslType", "PslType");
                bulkObject.ColumnMappings.Add("LetterDate", "LetterDate");
                bulkObject.ColumnMappings.Add("TerminationDate", "TerminationDate");
                bulkObject.ColumnMappings.Add("RepairDate", "RepairDate");
                bulkObject.ColumnMappings.Add("LaborHours", "LaborHours");
                bulkObject.ColumnMappings.Add("CatDeliveryDate", "CatDeliveryDate");
                bulkObject.ColumnMappings.Add("ServiceClaimAllowanceGroup", "ServiceClaimAllowanceGroup");
                bulkObject.ColumnMappings.Add("ProductSmuAgeRange1", "ProductSmuAgeRange1");
                bulkObject.ColumnMappings.Add("ProductSmuAgeRange2", "ProductSmuAgeRange2");
                bulkObject.ColumnMappings.Add("ProductSmuAgeRange3", "ProductSmuAgeRange3");
                bulkObject.ColumnMappings.Add("ProductSmuAgeRange4", "ProductSmuAgeRange4");

                sqlConnection.Open();
                bulkObject.BatchSize = 10000;
                bulkObject.BulkCopyTimeout = 0;
                bulkObject.WriteToServer(dataTable);
                sqlConnection.Close();
            }
        }

        public DataTable CreateTableComment(string path)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("PSLSN", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Comment", typeof(string)));

            var strcol = new string[] {
                "PSLSN",
                "Comment"
            };


            using (CsvReader csv = new CsvReader(new StreamReader(System.IO.File.OpenRead(path)), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (var i = 0; i < headers.Count(); i++)
                    {
                        dataRow[strcol[i]] = csv[i].ToString();
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private void TruncateComment()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE Comment");

            }
        }

        public void BulkUpdateComment(DataTable dataTable, string mainConnectionString)
        {
            if (dataTable.Rows.Count != 0)
            {
                TruncateComment();

                string connection = mainConnectionString;

                SqlConnection sqlConnection = new SqlConnection(connection);

                SqlBulkCopy bulkObject = new SqlBulkCopy(sqlConnection) { DestinationTableName = "Comment" };

                bulkObject.ColumnMappings.Add("PSLSN", "PSLSN");
                bulkObject.ColumnMappings.Add("Comment", "Comment");

                sqlConnection.Open();
                bulkObject.BatchSize = 10000;
                bulkObject.BulkCopyTimeout = 0;
                bulkObject.WriteToServer(dataTable);
                sqlConnection.Close();
            }
        }


        public DataTable CreateTableLocation(string path)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("JCode", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DealerDescription", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Area", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Region", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SalesOffice", typeof(string)));

            var strcol = new string[] {
                "JCode",
                "DealerDescription",
                "Area",
                "Region",
                "SalesOffice"
            };

            using (CsvReader csv = new CsvReader(new StreamReader(System.IO.File.OpenRead(path)), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (var i = 0; i < headers.Count(); i++)
                    {
                        dataRow[strcol[i]] = csv[i].ToString();
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private void TruncateLocation()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE Location2");

            }
        }

        public void BulkUpdateLocation(DataTable dataTable, string mainConnectionString)
        {
            if (dataTable.Rows.Count != 0)
            {
                TruncateLocation();

                string connection = mainConnectionString;

                SqlConnection sqlConnection = new SqlConnection(connection);

                SqlBulkCopy bulkObject = new SqlBulkCopy(sqlConnection) { DestinationTableName = "Location2" };

                bulkObject.ColumnMappings.Add("JCode", "JCode");
                bulkObject.ColumnMappings.Add("DealerDescription", "DealerDescription");
                bulkObject.ColumnMappings.Add("Area", "Area");
                bulkObject.ColumnMappings.Add("Region", "Region");
                bulkObject.ColumnMappings.Add("SalesOffice", "SalesOffice");

                sqlConnection.Open();
                bulkObject.BatchSize = 10000;
                bulkObject.BulkCopyTimeout = 0;
                bulkObject.WriteToServer(dataTable);
                sqlConnection.Close();
            }
        }


        public DataTable CreateTableWarrantyList(string path)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("WarrantyClaim", typeof(string)));
            dataTable.Columns.Add(new DataColumn("TransactionType", typeof(string)));
            dataTable.Columns.Add(new DataColumn("TransactionDescription", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PostingDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("RegistrationNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("RegistrationDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("Currency", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SettlementNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SettlementDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("Status", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SerialNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SalesOffice", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Area", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Region", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ServiceOrder", typeof(string)));
            dataTable.Columns.Add(new DataColumn("JobControl", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ClaimClass", typeof(string)));
            dataTable.Columns.Add(new DataColumn("InvoicedParts", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("InvoicedLabor", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("InvoicedTravel", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("InvoicedVehicle", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("InvoicedMisc", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("InvoicedTotal", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("ExpectedSettledParts", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("ExpectedSettledLabor", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("ExpectedSettledTravel", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("ExpectedSettledVehicle", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("ExpectedSettledMisc", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("ClaimedParts", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("ClaimedLabor", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("ClaimedTravel", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("ClaimedVehicle", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("ClaimedMisc", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("ClaimedTotal", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("SettledTotal", typeof(decimal)));

            var strcol = new string[] {
                "WarrantyClaim",
                "TransactionType",
                "TransactionDescription",
                "PostingDate",
                "RegistrationNumber",
                "RegistrationDate",
                "Currency",
                "SettlementNumber",
                "SettlementDate",
                "Status",
                "SerialNumber",
                "CustomerName",
                "SalesOffice",
                "Area",
                "Region",
                "ServiceOrder",
                "JobControl",
                "ClaimClass",
                "InvoicedParts",
                "InvoicedLabor",
                 "InvoicedTravel",
                "InvoicedVehicle",
                "InvoicedMisc",
                "InvoicedTotal",
                "ExpectedSettledParts",
                "ExpectedSettledLabor",
                "ExpectedSettledTravel",
                "ExpectedSettledVehicle",
                "ExpectedSettledMisc",
                "ClaimedParts",
                "ClaimedLabor",
                "ClaimedTravel",
                "ClaimedVehicle",
                "ClaimedMisc",
                "ClaimedTotal",
                "SettledTotal"
            };

            using (CsvReader csv = new CsvReader(new StreamReader(System.IO.File.OpenRead(path)), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (var i = 0; i < headers.Count(); i++)
                    {
                        if(i == 3)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if(i == 5)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if (i == 8)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else
                        {
                            dataRow[strcol[i]] = csv[i].ToString();
                        }
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private void TruncateWarrantyList()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE WarrantyList");

            }
        }

        public void BulkUpdateWarrantyList(DataTable dataTable, string mainConnectionString)
        {
            if (dataTable.Rows.Count != 0)
            {
                TruncateWarrantyList();

                string connection = mainConnectionString;

                SqlConnection sqlConnection = new SqlConnection(connection);

                SqlBulkCopy bulkObject = new SqlBulkCopy(sqlConnection) { DestinationTableName = "WarrantyList" };

                bulkObject.ColumnMappings.Add("ServiceOrder", "ServiceOrder");
                bulkObject.ColumnMappings.Add("WarrantyClaim", "WarrantyClaim");
                bulkObject.ColumnMappings.Add("PostingDate", "PostingDate");
                bulkObject.ColumnMappings.Add("Currency", "Currency");
                bulkObject.ColumnMappings.Add("TransactionType", "TransactionType");
                bulkObject.ColumnMappings.Add("TransactionDescription", "TransactionDescription");
                bulkObject.ColumnMappings.Add("RegistrationNumber", "RegistrationNumber");
                bulkObject.ColumnMappings.Add("RegistrationDate", "RegistrationDate");
                bulkObject.ColumnMappings.Add("SettlementNumber", "SettlementNumber");
                bulkObject.ColumnMappings.Add("SettlementDate", "SettlementDate");
                bulkObject.ColumnMappings.Add("Status", "Status");
                bulkObject.ColumnMappings.Add("SerialNumber", "SerialNumber");
                bulkObject.ColumnMappings.Add("CustomerName", "CustomerName");
                bulkObject.ColumnMappings.Add("SalesOffice", "SalesOffice");
                bulkObject.ColumnMappings.Add("Area", "Area");
                bulkObject.ColumnMappings.Add("Region", "Region");
                bulkObject.ColumnMappings.Add("JobControl", "JobControl");
                bulkObject.ColumnMappings.Add("ClaimClass", "ClaimClass");
                bulkObject.ColumnMappings.Add("InvoicedParts", "InvoicedParts");
                bulkObject.ColumnMappings.Add("InvoicedLabor", "InvoicedLabor");
                bulkObject.ColumnMappings.Add("InvoicedVehicle", "InvoicedVehicle");
                bulkObject.ColumnMappings.Add("InvoicedTravel", "InvoicedTravel");
                bulkObject.ColumnMappings.Add("InvoicedMisc", "InvoicedMisc");
                bulkObject.ColumnMappings.Add("InvoicedTotal", "InvoicedTotal");
                bulkObject.ColumnMappings.Add("ExpectedSettledParts", "ExpectedSettledParts");
                bulkObject.ColumnMappings.Add("ExpectedSettledLabor", "ExpectedSettledLabor");
                bulkObject.ColumnMappings.Add("ExpectedSettledVehicle", "ExpectedSettledVehicle");
                bulkObject.ColumnMappings.Add("ExpectedSettledTravel", "ExpectedSettledTravel");
                bulkObject.ColumnMappings.Add("ExpectedSettledMisc", "ExpectedSettledMisc");
                bulkObject.ColumnMappings.Add("ClaimedParts", "ClaimedParts");
                bulkObject.ColumnMappings.Add("ClaimedLabor", "ClaimedLabor");
                bulkObject.ColumnMappings.Add("ClaimedVehicle", "ClaimedVehicle");
                bulkObject.ColumnMappings.Add("ClaimedTravel", "ClaimedTravel");
                bulkObject.ColumnMappings.Add("ClaimedMisc", "ClaimedMisc");
                bulkObject.ColumnMappings.Add("ClaimedTotal", "ClaimedTotal");
                bulkObject.ColumnMappings.Add("SettledTotal", "SettledTotal");

                sqlConnection.Open();
                bulkObject.BatchSize = 10000;
                bulkObject.BulkCopyTimeout = 0;
                bulkObject.WriteToServer(dataTable);
                sqlConnection.Close();
            }
        }



        public DataTable CreateTableDPPMSummary(string path)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("SRNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Title", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ProblemDescription", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SRNotes", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Status", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SubStatus", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CPINumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DealerContactName", typeof(string)));
            dataTable.Columns.Add(new DataColumn("TechRep", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Z_Industry", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PrimeProductGroupName", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CountAffectedUnit", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("DPPMQuickScore", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("TotalDetailedScore", typeof(decimal)));
            dataTable.Columns.Add(new DataColumn("DealerPPMCurrentStatus", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PCA", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PCADate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("ICA", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ICADate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("CreateOn", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("DateDealerOpen", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("DateLastUpdated", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("DateDealerClosed", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("DealerCode", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DealershipName", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ParentDealerCode", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CaterpillarPPMStatus", typeof(string)));
            dataTable.Columns.Add(new DataColumn("GroupName", typeof(string)));

            var strcol = new string[] {
                "SRNumber",
                "Title",
                "ProblemDescription",
                "SRNotes",
                "Status",
                "SubStatus",
                "CPINumber",
                "DealerContactName",
                "TechRep",
                "Z_Industry",
                "PrimeProductGroupName",
                "CountAffectedUnit",
                "DPPMQuickScore",
                "TotalDetailedScore",
                "DealerPPMCurrentStatus",
                "PCA",
                "PCADate",
                "ICA",
                "ICADate",
                "CreateOn",
                "DateDealerOpen",
                 "DateLastUpdated",
                "DateDealerClosed",
                "DealerCode",
                "DealershipName",
                "ParentDealerCode",
                "CaterpillarPPMStatus",
                "GroupName"
                
            };

            using (CsvReader csv = new CsvReader(new StreamReader(System.IO.File.OpenRead(path)), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (var i = 0; i < headers.Count(); i++)
                    {
                        if (i == 11)
                        {
                            if (csv[i] != "")
                            {
                                var convertToDecimal = Convert.ToDecimal(csv[i], null);
                                var roundValue = Math.Round(convertToDecimal, 2);
                                dataRow[strcol[i]] = roundValue;
                            }
                            else
                            {
                                dataRow[strcol[i]] = 0;
                            }
                        }
                        else if (i == 12)
                        {
                            if (csv[i] != "")
                            {
                                var convertToDecimal = Convert.ToDecimal(csv[i], null);
                                var roundValue = Math.Round(convertToDecimal, 2);
                                dataRow[strcol[i]] = roundValue;
                            }
                            else
                            {
                                dataRow[strcol[i]] = 0;
                            }
                        }
                       else  if (i == 13)
                        {
                            if (csv[i] != "")
                            {
                                var convertToDecimal = Convert.ToDecimal(csv[i], null);
                                var roundValue = Math.Round(convertToDecimal, 2);
                                dataRow[strcol[i]] = roundValue;
                            }
                            else
                            {
                                dataRow[strcol[i]] = 0;
                            }
                        }
                        else if (i == 16)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if(i == 18)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if(i == 19)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if(i == 20)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if(i == 21)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if(i == 22)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else
                        {
                            dataRow[strcol[i]] = csv[i].ToString();
                        }
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private void TruncateDPPMSummary()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE DPPMSummary");

            }
        }

        public void BulkUpdateDPPMSummary(DataTable dataTable, string mainConnectionString)
        {
            if (dataTable.Rows.Count != 0)
            {
                TruncateDPPMSummary();

                string connection = mainConnectionString;

                SqlConnection sqlConnection = new SqlConnection(connection);

                SqlBulkCopy bulkObject = new SqlBulkCopy(sqlConnection) { DestinationTableName = "DPPMSummary" };

                bulkObject.ColumnMappings.Add("SRNumber", "SRNumber");
                bulkObject.ColumnMappings.Add("Title", "Title");
                bulkObject.ColumnMappings.Add("ProblemDescription", "ProblemDescription");
                bulkObject.ColumnMappings.Add("SRNotes", "SRNotes");
                bulkObject.ColumnMappings.Add("Status", "Status");
                bulkObject.ColumnMappings.Add("SubStatus", "SubStatus");
                bulkObject.ColumnMappings.Add("CPINumber", "CPINumber");
                bulkObject.ColumnMappings.Add("DealerContactName", "DealerContactName");
                bulkObject.ColumnMappings.Add("TechRep", "TechRep");
                bulkObject.ColumnMappings.Add("Z_Industry", "Z_Industry");
                bulkObject.ColumnMappings.Add("PrimeProductGroupName", "PrimeProductGroupName");
                bulkObject.ColumnMappings.Add("CountAffectedUnit", "CountAffectedUnit");
                bulkObject.ColumnMappings.Add("DPPMQuickScore", "DPPMQuickScore");
                bulkObject.ColumnMappings.Add("TotalDetailedScore", "TotalDetailedScore");
                bulkObject.ColumnMappings.Add("DealerPPMCurrentStatus", "DealerPPMCurrentStatus");
                bulkObject.ColumnMappings.Add("PCA", "PCA");
                bulkObject.ColumnMappings.Add("PCADate", "PCADate");
                bulkObject.ColumnMappings.Add("ICA", "ICA");
                bulkObject.ColumnMappings.Add("ICADate", "ICADate");
                bulkObject.ColumnMappings.Add("CreateOn", "CreateOn");
                bulkObject.ColumnMappings.Add("DateDealerOpen", "DateDealerOpen");
                bulkObject.ColumnMappings.Add("DateLastUpdated", "DateLastUpdated");
                bulkObject.ColumnMappings.Add("DateDealerClosed", "DateDealerClosed");
                bulkObject.ColumnMappings.Add("DealerCode", "DealerCode");
                bulkObject.ColumnMappings.Add("DealershipName", "DealershipName");
                bulkObject.ColumnMappings.Add("ParentDealerCode", "ParentDealerCode");
                bulkObject.ColumnMappings.Add("CaterpillarPPMStatus", "CaterpillarPPMStatus");
                bulkObject.ColumnMappings.Add("GroupName", "GroupName");

                sqlConnection.Open();
                bulkObject.BatchSize = 10000;
                bulkObject.BulkCopyTimeout = 0;
                bulkObject.WriteToServer(dataTable);
                sqlConnection.Close();
            }
        }


        public DataTable CreateTableDPPMAffectedUnit(string path)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("DealerPPM", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CreatedOn", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("BrandAffiliation", typeof(string)));
            dataTable.Columns.Add(new DataColumn("BuildDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("Comment", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CreatedByDelegate", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CustomerName", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DateFailure", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("DateOfRepair", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("DeliveryDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("EventType", typeof(string)));
            dataTable.Columns.Add(new DataColumn("HoursAtFailure", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ModifiedBy", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ModifiedByDelegate", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ModifiedOn", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("Owner", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PrimaryAffectedUnit", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PrimeProductApplicationCategoryCode", typeof(int)));
            dataTable.Columns.Add(new DataColumn("PrimeProductApplicationCategoryName", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PrimeProductApplicationCode", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PrimeProductApplicationName", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PrimeProductFamily", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PrimeProductFamilyCode", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PrimeProductGeneralArrangmentNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PrimeProductModel", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PrimeProductSourceFacility", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ProductSNP", typeof(string)));
            dataTable.Columns.Add(new DataColumn("RecordCreatedOn", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("SerialNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ServiceMeterUnit", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SNP", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SourcePlantCode", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Status", typeof(string)));
            dataTable.Columns.Add(new DataColumn("StatusReason", typeof(string)));
            dataTable.Columns.Add(new DataColumn("WorkOrderNumber", typeof(string)));

            var strcol = new string[] {
                "DealerPPM",
                "CreatedOn",
                "BrandAffiliation",
                "BuildDate",
                "Comment",
                "CreatedBy",
                "CreatedByDelegate",
                "CustomerName",
                "DateFailure",
                "DateOfRepair",
                "DeliveryDate",
                "EventType",
                "HoursAtFailure",
                "ModifiedBy",
                "ModifiedByDelegate",
                "ModifiedOn",
                "Owner",
                "PrimaryAffectedUnit",
                "PrimeProductApplicationCategoryCode",
                "PrimeProductApplicationCategoryName",
                 "PrimeProductApplicationCode",
                "PrimeProductApplicationName",
                "PrimeProductFamily",
                "PrimeProductFamilyCode",
                "PrimeProductGeneralArrangmentNumber",
                "PrimeProductModel",
                "PrimeProductSourceFacility",
                "ProductSNP",
                "RecordCreatedOn",
                "SerialNumber",
                "ServiceMeterUnit",
                "SNP",
                "SourcePlantCode",
                "Status",
                "StatusReason",
                "WorkOrderNumber"
            };

            using (CsvReader csv = new CsvReader(new StreamReader(System.IO.File.OpenRead(path)), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (var i = 0; i < headers.Count(); i++)
                    {
                        if (i == 0)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 1)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if (i == 2)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 3)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if (i == 4)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 5)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 6)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 7)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 8)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if (i == 9)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if (i == 10)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if (i == 11)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 12)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 13)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 14)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 15)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if (i == 16)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 17)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 18)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToInt32(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = 0;
                            }
                        }
                        else if (i == 19)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 20)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 21)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 22)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 23)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 24)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = 0;
                            }
                        }
                        else if (i == 25)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 26)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 27)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 28)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if (i == 29)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 30)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 31)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 32)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 33)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 34)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                        else if (i == 35)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = csv[i].ToString();
                            }
                            else
                            {
                                dataRow[strcol[i]] = "";
                            }
                        }
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private void TruncateDPPMAffectedUnit()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE DPPMAffectedUnit");

            }
        }

        public void BulkUpdateDPPMAffectedUnit(DataTable dataTable, string mainConnectionString)
        {
            if (dataTable.Rows.Count != 0)
            {
                TruncateDPPMAffectedUnit();

                string connection = mainConnectionString;

                SqlConnection sqlConnection = new SqlConnection(connection);

                SqlBulkCopy bulkObject = new SqlBulkCopy(sqlConnection) { DestinationTableName = "DPPMAffectedUnit" };

                bulkObject.ColumnMappings.Add("DealerPPM", "DealerPPM");
                bulkObject.ColumnMappings.Add("CreatedOn", "CreatedOn");
                bulkObject.ColumnMappings.Add("BrandAffiliation", "BrandAffiliation");
                bulkObject.ColumnMappings.Add("BuildDate", "BuildDate");
                bulkObject.ColumnMappings.Add("Comment", "Comment");
                bulkObject.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulkObject.ColumnMappings.Add("CreatedByDelegate", "CreatedByDelegate");
                bulkObject.ColumnMappings.Add("CustomerName", "CustomerName");
                bulkObject.ColumnMappings.Add("DateFailure", "DateFailure");
                bulkObject.ColumnMappings.Add("DateOfRepair", "DateOfRepair");
                bulkObject.ColumnMappings.Add("DeliveryDate", "DeliveryDate");
                bulkObject.ColumnMappings.Add("EventType", "EventType");
                bulkObject.ColumnMappings.Add("HoursAtFailure", "HoursAtFailure");
                bulkObject.ColumnMappings.Add("ModifiedBy", "ModifiedBy");
                bulkObject.ColumnMappings.Add("ModifiedByDelegate", "ModifiedByDelegate");
                bulkObject.ColumnMappings.Add("ModifiedOn", "ModifiedOn");
                bulkObject.ColumnMappings.Add("Owner", "Owner");
                bulkObject.ColumnMappings.Add("PrimaryAffectedUnit", "PrimaryAffectedUnit");
                bulkObject.ColumnMappings.Add("PrimeProductApplicationCategoryCode", "PrimeProductApplicationCategoryCode");
                bulkObject.ColumnMappings.Add("PrimeProductApplicationCategoryName", "PrimeProductApplicationCategoryName");
                bulkObject.ColumnMappings.Add("PrimeProductApplicationCode", "PrimeProductApplicationCode");
                bulkObject.ColumnMappings.Add("PrimeProductApplicationName", "PrimeProductApplicationName");
                bulkObject.ColumnMappings.Add("PrimeProductFamily", "PrimeProductFamily");
                bulkObject.ColumnMappings.Add("PrimeProductFamilyCode", "PrimeProductFamilyCode");
                bulkObject.ColumnMappings.Add("PrimeProductGeneralArrangmentNumber", "PrimeProductGeneralArrangmentNumber");
                bulkObject.ColumnMappings.Add("PrimeProductModel", "PrimeProductModel");
                bulkObject.ColumnMappings.Add("PrimeProductSourceFacility", "PrimeProductSourceFacility");
                bulkObject.ColumnMappings.Add("ProductSNP", "ProductSNP");
                bulkObject.ColumnMappings.Add("RecordCreatedOn", "RecordCreatedOn");
                bulkObject.ColumnMappings.Add("SerialNumber", "SerialNumber");
                bulkObject.ColumnMappings.Add("ServiceMeterUnit", "ServiceMeterUnit");
                bulkObject.ColumnMappings.Add("SNP", "SNP");
                bulkObject.ColumnMappings.Add("SourcePlantCode", "SourcePlantCode");
                bulkObject.ColumnMappings.Add("Status", "Status");
                bulkObject.ColumnMappings.Add("StatusReason", "StatusReason");
                bulkObject.ColumnMappings.Add("WorkOrderNumber", "WorkOrderNumber");

                sqlConnection.Open();
                bulkObject.BatchSize = 10000;
                bulkObject.BulkCopyTimeout = 0;
                bulkObject.WriteToServer(dataTable);
                sqlConnection.Close();
            }
        }


        public DataTable CreateTableDPPMAffectedPart(string path)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("PartNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("CreatedOn", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
            dataTable.Columns.Add(new DataColumn("DealerPPM", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PartHours", typeof(int)));
            dataTable.Columns.Add(new DataColumn("Parts", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PDCode", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PrimaryPart", typeof(string)));
            dataTable.Columns.Add(new DataColumn("FailureQuantity", typeof(int)));
           

            var strcol = new string[] {
                "PartNumber",
                "CreatedOn",
                "CreatedBy",
                "DealerPPM",
                "PartHours",
                "Parts",
                "PDCode",
                "PrimaryPart",
                "FailureQuantity"
            };

            using (CsvReader csv = new CsvReader(new StreamReader(System.IO.File.OpenRead(path)), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (var i = 0; i < headers.Count(); i++)
                    {
                        if(i == 1)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else if(i == 4)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToInt32(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = 0;
                            }
                        }
                        else if(i == 8)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToInt32(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = 0;
                            }
                        }
                        else
                        {
                            dataRow[strcol[i]] = csv[i].ToString();
                        }
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private void TruncateDPPMAffectedPart()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE DPPMAffectedPart");

            }
        }

        public void BulkUpdateDPPMAffectedPart(DataTable dataTable, string mainConnectionString)
        {
            if (dataTable.Rows.Count != 0)
            {
                TruncateDPPMAffectedPart();

                string connection = mainConnectionString;

                SqlConnection sqlConnection = new SqlConnection(connection);

                SqlBulkCopy bulkObject = new SqlBulkCopy(sqlConnection) { DestinationTableName = "DPPMAffectedPart" };

                bulkObject.ColumnMappings.Add("PartNumber", "PartNumber");
                bulkObject.ColumnMappings.Add("CreatedOn", "CreatedOn");
                bulkObject.ColumnMappings.Add("CreatedBy", "CreatedBy");
                bulkObject.ColumnMappings.Add("DealerPPM", "DealerPPM");
                bulkObject.ColumnMappings.Add("PartHours", "PartHours");
                bulkObject.ColumnMappings.Add("Parts", "Parts");
                bulkObject.ColumnMappings.Add("PDCode", "PDCode");
                bulkObject.ColumnMappings.Add("PrimaryPart", "PrimaryPart");
                bulkObject.ColumnMappings.Add("FailureQuantity", "FailureQuantity");

                sqlConnection.Open();
                bulkObject.BatchSize = 10000;
                bulkObject.BulkCopyTimeout = 0;
                bulkObject.WriteToServer(dataTable);
                sqlConnection.Close();
            }
        }


        public DataTable CreateTableSIMSErrorSummary(string path)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("ErrorDescription", typeof(string)));
            dataTable.Columns.Add(new DataColumn("RepairingDealer", typeof(string)));
            dataTable.Columns.Add(new DataColumn("WorkOrder", typeof(string)));
            dataTable.Columns.Add(new DataColumn("WorkOrderSegment", typeof(int)));
            dataTable.Columns.Add(new DataColumn("SerialNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("RepairDate", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("PartCausingFailure", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PartCausingFailureDescription", typeof(string)));
            dataTable.Columns.Add(new DataColumn("GroupNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("GroupDescription", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Comment", typeof(string)));


            var strcol = new string[] {
                "ErrorDescription",
                "RepairingDealer",
                "WorkOrder",
                "WorkOrderSegment",
                "SerialNumber",
                "RepairDate",
                "PartCausingFailure",
                "PartCausingFailureDescription",
                "GroupNumber",
                "GroupDescription",
                "Comment",
            };

            using (CsvReader csv = new CsvReader(new StreamReader(System.IO.File.OpenRead(path)), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (var i = 0; i < headers.Count(); i++)
                    {
                        if(i == 3)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToInt32(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = 0;
                            }
                        }
                        else if(i == 5)
                        {
                            if (csv[i] != "")
                            {
                                dataRow[strcol[i]] = Convert.ToDateTime(csv[i].ToString());
                            }
                            else
                            {
                                dataRow[strcol[i]] = DBNull.Value;
                            }
                        }
                        else
                        {
                            dataRow[strcol[i]] = csv[i].ToString();
                        }
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private void TruncateSIMSErrorSummary()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE SIMSErrorSummary");

            }
        }

        public void BulkUpdateSIMSErrorSummary(DataTable dataTable, string mainConnectionString)
        {
            if (dataTable.Rows.Count != 0)
            {
                TruncateSIMSErrorSummary();

                string connection = mainConnectionString;

                SqlConnection sqlConnection = new SqlConnection(connection);

                SqlBulkCopy bulkObject = new SqlBulkCopy(sqlConnection) { DestinationTableName = "SIMSErrorSummary" };

                bulkObject.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                bulkObject.ColumnMappings.Add("RepairingDealer", "RepairingDealer");
                bulkObject.ColumnMappings.Add("WorkOrder", "WorkOrder");
                bulkObject.ColumnMappings.Add("WorkOrderSegment", "WorkOrderSegment");
                bulkObject.ColumnMappings.Add("SerialNumber", "SerialNumber");
                bulkObject.ColumnMappings.Add("RepairDate", "RepairDate");
                bulkObject.ColumnMappings.Add("PartCausingFailure", "PartCausingFailure");
                bulkObject.ColumnMappings.Add("PartCausingFailureDescription", "PartCausingFailureDescription");
                bulkObject.ColumnMappings.Add("GroupNumber", "GroupNumber");
                bulkObject.ColumnMappings.Add("GroupDescription", "GroupDescription");
                bulkObject.ColumnMappings.Add("Comment", "Comment");

                sqlConnection.Open();
                bulkObject.BatchSize = 10000;
                bulkObject.BulkCopyTimeout = 0;
                bulkObject.WriteToServer(dataTable);
                sqlConnection.Close();
            }
        }


        public DataTable CreateTableOrganization(string path)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("SalesOfficeCode", typeof(string)));
            dataTable.Columns.Add(new DataColumn("SalesOfficeDescription", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Region", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Area", typeof(string)));


            var strcol = new string[] {
                "SalesOfficeCode",
                "SalesOfficeDescription",
                "Region",
                "Area"
            };

            using (CsvReader csv = new CsvReader(new StreamReader(System.IO.File.OpenRead(path)), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (var i = 0; i < headers.Count(); i++)
                    {
                        dataRow[strcol[i]] = csv[i].ToString();
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private void TruncateOrganization()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE Organization");

            }
        }

        public void BulkUpdateOrganization(DataTable dataTable, string mainConnectionString)
        {
            if (dataTable.Rows.Count != 0)
            {
                TruncateOrganization();

                string connection = mainConnectionString;

                SqlConnection sqlConnection = new SqlConnection(connection);

                SqlBulkCopy bulkObject = new SqlBulkCopy(sqlConnection) { DestinationTableName = "Organization" };

                bulkObject.ColumnMappings.Add("SalesOfficeCode", "SalesOfficeCode");
                bulkObject.ColumnMappings.Add("SalesOfficeDescription", "SalesOfficeDescription");
                bulkObject.ColumnMappings.Add("Region", "Region");
                bulkObject.ColumnMappings.Add("Area", "Area");

                sqlConnection.Open();
                bulkObject.BatchSize = 10000;
                bulkObject.BulkCopyTimeout = 0;
                bulkObject.WriteToServer(dataTable);
                sqlConnection.Close();
            }
        }

        public DataTable CreateTablePSLPart(string path)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("PSLNo", typeof(string)));
            dataTable.Columns.Add(new DataColumn("PartNumber", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Description", typeof(string)));
            dataTable.Columns.Add(new DataColumn("RequiredQuantity", typeof(string)));

            var strcol = new string[] {
                "PSLNo",
                "PartNumber",
                "Description",
                "RequiredQuantity"
            };

            using (CsvReader csv = new CsvReader(new StreamReader(System.IO.File.OpenRead(path)), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
                while (csv.ReadNextRecord())
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (var i = 0; i < headers.Count(); i++)
                    {
                        dataRow[strcol[i]] = csv[i].ToString();
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private void TruncatePSLPart()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE PSLPart");

            }
        }

        public void BulkUpdatePSLPart(DataTable dataTable, string mainConnectionString)
        {
            if (dataTable.Rows.Count != 0)
            {
                TruncatePSLPart();

                string connection = mainConnectionString;

                SqlConnection sqlConnection = new SqlConnection(connection);

                SqlBulkCopy bulkObject = new SqlBulkCopy(sqlConnection) { DestinationTableName = "PSLPart" };

                bulkObject.ColumnMappings.Add("PSLNo", "PSLNo");
                bulkObject.ColumnMappings.Add("PartNumber", "PartNumber");
                bulkObject.ColumnMappings.Add("Description", "Description");
                bulkObject.ColumnMappings.Add("RequiredQuantity", "RequiredQuantity");

                sqlConnection.Open();
                bulkObject.BatchSize = 10000;
                bulkObject.BulkCopyTimeout = 0;
                bulkObject.WriteToServer(dataTable);
                sqlConnection.Close();
            }
        }
    }
}
