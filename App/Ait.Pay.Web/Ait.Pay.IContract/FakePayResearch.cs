using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ait.Pay.IContract
{
    class FakePayResearch : IPayResearch
    {
        #region Data
        class Data
        {

            //-------- kt ---------------
            public static List<PayServiceItem> ResearchList = new List<PayServiceItem>
            {

                 new PayServiceItem { Id = "111", Value = "Компьютерная томография", IsFolder = true  },
                 new PayServiceItem { Id = "112", Value = "головы" , IsFolder = true, ParentId="111" },
                 new PayServiceItem { Id = "113", Value = "сердца и сосудов"  , IsFolder = true, ParentId="111"},
                 new PayServiceItem { Id = "114", Value = "костей" , IsFolder = true, ParentId="111" },

                 new PayServiceItem
                 {
                     Id = "12",
                     Value = "глаз",
                     ParentId = "112"

                 },
                 new PayServiceItem
                 {
                     Id = "13",
                     Value = "головного мозга",
                     ParentId = "112"
                 },
                 new PayServiceItem
                 {
                     Id = "14",
                     Value = "гортани",
                     ParentId = "112"
                 },
                 new PayServiceItem
                 {
                     Id = "15",
                     Value = "придаточных пазух носа",
                     ParentId = "112"
                 },
                 new PayServiceItem
                 {
                     Id = "16",
                     Value = "трахеи",
                     ParentId = "112"
                 },
                 new PayServiceItem
                 {
                     Id = "17",
                     Value = "сердца (кальций-скоринг)",
                     ParentId = "112"
                 },

                 new PayServiceItem
                 {
                     Id = "18",
                     Value = "височных костей",
                     ParentId = "114"
                 },

                 new PayServiceItem
                 {
                     Id = "19",
                     Value = "костей таза",
                     ParentId = "114"
                 },
                 new PayServiceItem
                 {
                     Id = "20",
                     Value = "плотности костной ткани",
                     ParentId=  "114"
                 },

                 //-------- МРТ ---------------


                 new PayServiceItem { Id = "121", Value = "МРТ", IsFolder = true  },
                 new PayServiceItem { Id = "122", Value = "головы и шеи" , IsFolder = true, ParentId="121" },
                 new PayServiceItem { Id = "123", Value = "брюшной полости и таза" , IsFolder = true, ParentId="121" },


                 new PayServiceItem
                 {
                     Id = "21",
                     Value = "гипофиза",
                     ParentId = "122"
                 },
                 new PayServiceItem
                 {
                     Id = "22",
                     Value = "головного мозга",
                     ParentId = "122"
                 },
                 new PayServiceItem
                 {
                     Id = "23",
                     Value = " головного мозга и придаточных пазух носа",
                     ParentId = "122"
                 },

                 new PayServiceItem
                 {
                     Id = "24",
                     Value = "брюшного отдела аорты и почечных артерий",
                     ParentId = "123"
                 },

                 new PayServiceItem
                 {
                     Id = "25",
                     Value = "желчного пузыря",
                     ParentId = "123"
                 }
                 ,


                 new PayServiceItem
                 {
                     Id = "26",
                     Value = "органов брюшной полости",
                     ParentId = "123"
                 }
                 ,


                 new PayServiceItem
                 {
                     Id = "27",
                     Value = "почек и надпочечников",
                     ParentId = "123"
                 }
                 ,


                 //------------- Рентген ------------------------


                new PayServiceItem { Id = "131", Value = "Рентген", IsFolder= true  },
                new PayServiceItem { Id = "133", Value = "внутренних органов", IsFolder= true, ParentId = "131"  },
                new PayServiceItem { Id = "134", Value = "внутренних 1", IsFolder= true, ParentId = "131"  },


                 new PayServiceItem
                 {
                     Id = "38",
                     Value = "брюшной полости (обзорная)",
                     ParentId = "134"
                 }
                 ,

                 new PayServiceItem
                 {
                     Id = "30",
                     Value = "брюшной полости (обзорная)",
                     ParentId = "133"
                 }
                 ,

                 new PayServiceItem
                 {
                     Id = "31",
                     Value = "гортани и трахеи",
                     ParentId = "133"
                 }
                 ,

                 new PayServiceItem
                 {
                     Id = "32",
                     Value = "двенадцатиперстной кишки",
                     ParentId = "133"
                 }
                 ,

                 new PayServiceItem
                 {
                     Id = "33",
                     Value = "диафрагмы",
                     ParentId = "133"
                 }
                 ,

                 new PayServiceItem
                 {
                     Id = "34",
                     Value = "мягких тканей лимф.узлов, флеболитов,артер. обызвествл.",
                     ParentId = "133"
                 }
                 ,


                 new PayServiceItem
                 {
                     Id = "35",
                     Value = "органов грудной клетки (прицельная)",
                     ParentId = "133"
                 },

                 new PayServiceItem
                 {
                     Id = "36",
                     Value = "сердца с контрастированием пищевода (3 проекции)",
                     ParentId = "133"
                 },

                 new PayServiceItem
                 {
                     Id = "37",
                     Value = "толстой кишки",
                     ParentId = "133"
                 }
                 ,

                 new PayServiceItem { Id = "141", Value = "Функциональная диагностика", IsFolder = true  },


                 new PayServiceItem
                 {
                     Id = "40",
                     Value = "Тредмил-тест",
                     ParentId = "141"
                 }
                 ,

                 new PayServiceItem
                 {
                     Id = "41",
                     Value = "ЭКГ",
                     ParentId = "141"
                 },

                 new PayServiceItem
                 {
                     Id = "42",
                     Value = "ЭКГ с нагрузочной пробой",
                     ParentId = "141"
                 },

                 new PayServiceItem
                 {
                     Id = "43",
                     Value = "ЭЭГ",
                     ParentId = "141"
                 },

                 //-------------------------   UZI ---------------------

                new PayServiceItem { Id = "150", Value = "УЗИ" , IsFolder = true },
                new PayServiceItem { Id = "151", Value = "грудной клетки",IsFolder = true,ParentId = "150"  },
                new PayServiceItem { Id = "152", Value = "для беременных", IsFolder = true,ParentId = "150"  },


                 new PayServiceItem
                 {
                     Id = "50",
                     Value = "плевральной полости легких",
                     ParentId = "151"
                 },

                 new PayServiceItem
                 {
                     Id = "51",
                     Value = "средостения",
                     ParentId = "151"
                 },

                 new PayServiceItem
                 {
                     Id = "52",
                     Value = "сердца",
                     ParentId = "151"
                 }
                 ,
                 new PayServiceItem
                 {
                     Id = "53",
                     Value = "в 1ом триместре беременности",
                     ParentId = "152"
                 }
                 ,

                 new PayServiceItem
                 {
                     Id = "54",
                     Value = "в 1ом триместре при многоплодной беременности",
                     ParentId = "152"
                 },


                 new PayServiceItem
                 {
                     Id = "55",
                     Value = "во 2-3 триместре беременности с оценкой органов плода при многоплодной беременности",
                     ParentId = "152"
                 }
                 ,

                 new PayServiceItem
                 {
                     Id = "56",
                     Value = "скрининг 2-3 триместра при многоплодной беременности",
                     ParentId = "152"
                 }
                 ,
                 new PayServiceItem
                 {
                     Id = "57",
                     Value = "УЗДГ маточно-плацентарного кровотока одноплодной беременности",
                     ParentId = "152"
                 }
            };



        }
        #endregion


        public async Task<List<PayServiceItem>> GetResearchList(PayCriteria criteria)
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(2000);
                return Data.ResearchList;
            });
        }


        public async Task<List<PayIdValue>> GetResearchVisitDays(PayCriteria criteria)
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(2000);

                return new List<PayIdValue>
                {
                    "1".PayValue("2016-10-01").SetPropValue("Text", "09:00-18:00"),
                    "2".PayValue("2016-10-1" ).SetPropValue("Text", "10:00-18:00"),
                    "3".PayValue("2016-10-10").SetPropValue("Text", "10:00-18:00"),
                    "4".PayValue("2016-10-12").SetPropValue("Text", "10:00-18:00")
                };

            });
        }



        public async Task<List<PayIdValue>> GetResearchVisitTimes(PayCriteria criteria)
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(2000);

                return new List<PayIdValue>
                {
                    "1".PayValue("10:10"),
                    "2".PayValue("10:20"),
                    "3".PayValue("10:30"),
                    "4".PayValue("10:40"),
                    "5".PayValue("11:30"),
                    "6".PayValue("12:43"),
                    "7".PayValue("17:20")
                };
            });
        }

        public Task<PayResearchLocation> GetResearchLocation(PayCriteria criteria)
        {
            return Task.Run(() =>
          {
              return new PayResearchLocation
              {

                  Address = new PayIdValue { Value = "111539, Москва, ул. Вешняковская, 23" },
                  Lpu = new PayIdValue { Id = "GKB15", Value = "Городская клиническая больница №15 имени О. М. Филатова " },


                  About = @"Колоноскопия – это медицинская процедура, проводимая для оценки состояния поверхности кишки.
 Слизь, жидкость, различные наложения на стенках кишки могут скрыть очаг заболевания, изменив
визуальную картинку - источник информации в диагностике заболеваний кишки.
 Подготовка - важный этап в обследовании, и ее качество - зона Вашей ответственности.
 ДЛЯ КАЧЕСТВЕННОЙ ПОДГОТОВКИ И ЛЕГКОГО ПРОХОЖДЕНИЯ ПРОЦЕДУРЫ ВАМ
ПОНАДОБЯТСЯ: положительный настрой, соблюдение «бесшлаковой» диеты за 2 ДНЯ до процедуры,
соблюдение «жидкостной» диеты за 24 ЧАСА до исследования и лекарственный препарат для комфортной
очистки кишечника ФОРТРАНС или ФЛИТ ФОСФО-СОДА.",

                  Service = new PayServiceItem { Id = "1", Value = "Колоноскопия", Price = "1000-2000рур" },

                  Rooms = new List<PayRoom>
                  {
                     new PayRoom
                     {
                         Value = "каб.505 эт.2",
                         Times = new List<PayIdValue>
                         {
                             "Пн, Ср".PayValue("08:00-12:48"),
                             "Сб".PayValue("08:00-09:00")
                         }
                     },
                     new PayRoom
                     {
                         Value = "каб.102",
                         Times = new List<PayIdValue>
                         {
                             "Вт, Чт".PayValue("14:00-18:00")
                         }
                     }
                }
              };
          });
        }
    }
}
