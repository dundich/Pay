using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Ait.Pay.IContract
{
    class FakePayDoctor : IPayDoctor
    {
        #region Data
        class Data
        {
            public static List<PayIdValue> SpecList = new List<PayIdValue>
            {
                 new PayIdValue
                 {
                     Id = "1",
                     Value = "Акушер"
                 },
                 new PayIdValue
                 {
                     Id = "2",
                     Value = "Аллерголог"
                 },
                 new PayIdValue
                 {
                     Id = "3",
                     Value = "Андролог"
                 },
                 new PayIdValue
                 {
                     Id = "4",
                     Value = "Анестезиолог"
                 },
                 new PayIdValue
                 {
                     Id = "5",
                     Value = "Венеролог"
                 },
                 new PayIdValue
                 {
                     Id = "6",
                     Value = "Гастроэнтеролог"
                 },
                 new PayIdValue
                 {
                     Id = "7",
                     Value = "Гематолог"
                 },
                 new PayIdValue
                 {
                     Id = "8",
                     Value = "Генетик"
                 },
                 new PayIdValue
                 {
                     Id = "9",
                     Value = "Гепатолог"
                 },
                 new PayIdValue
                 {
                     Id = "10",
                     Value = "Гинеколог"
                 },
                 new PayIdValue
                 {
                     Id = "11",
                     Value = "Гомеопат"
                 },
              };


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


            public static List<PayDoctor> DocList = new List<PayDoctor>
            {
                new PayDoctor
                {
                    Id = "1",
                    Value = "Андрющенко Ирина Сергеевна",
                    Avatar = @"Assets/imgs/doc1.jpg",
                    About =
@"<p class='hidden-sm hidden-xs'>
Опытный и грамотный мастер в сфере терапевтической стоматологии, специализируется на данной отрасли с 2008 года.
Выпускница Ташкентской медицинской академии.
За долгие годы студенческой учебы и практической деятельности Снежко К.В. 
посещала множество различных курсов по повышению квалификации, семинаров, 
мастер-классов и тренингов.</p>",
                    JobAge = 12,
                    Specialities = new List<PayIdValue> { "1".PayValue("Стоматолог") , "2".PayValue("Стоматолог-терапевт") },
                    Ranges = new List<string> { "Высшая категория" },
                    LpuList = new List<PayLpuLocation>
                    {
                        new PayLpuLocation
                        {
                            Address = new PayIdValue { Value= "111539, Москва, ул. Вешняковская, 23" },
                            Lpu = new PayIdValue { Id= "GKB15", Value = "Городская клиническая больница №15 имени О. М. Филатова "},

                            Services = new List<PayServiceItem>
                            {
                                new PayServiceItem { Id = "1", Value = "Художественная реставрация зубов", Price = "2 500 - 3 000" },
                                new PayServiceItem { Id = "2", Value = "Лечение кариеса", Price = "500" },
                                new PayServiceItem { Id = "3", Value = "Look at Me Now", Price = "3 120" },
                            },

                            Specification = "1".PayValue("Стоматолог"),

                            Rooms = new List<PayRoom>
                            {
                                new PayRoom
                                {
                                    Value = "каб.505 эт.2",
                                    Times = new List<PayIdValue>
                                    {
                                        "Пн, Вт, Ср".PayValue("08:00-12:48"),
                                        "Сб".PayValue("08:00-09:00")
                                    }
                                }
                            },

                            //SpecificationType = PayPackage.TYPE_DOCTOR,
                        }
                    }
                },
                new PayDoctor
                {
                    Id = "2", Value = "Бугаева Наталья Васильевна",
                    Avatar = @"Assets/imgs/doc2.jpeg",
                    About =
@"<p class='hidden-sm hidden-xs'>
Окончила Северо-Осетинский Государственный медицинский институт, клиническую ординатуру в Первом Московском государственном медицинском университете имени И. М. Сеченова, аспирантуру в Первом Московском государственном медицинском университете имени И. М. Сеченова.</p>",
                    Specialities = new List<PayIdValue> { "1".PayValue("Стоматолог") , "2".PayValue("Стоматолог-терапевт") },
                    Ranges = new List<string> { "Кандидат медицинских наук", "Заведующая отделением" },
                    LpuList = new List<PayLpuLocation>
                    {
                        new PayLpuLocation
                        {

                            Services = new List<PayServiceItem>
                            {
                                new PayServiceItem { Id = "1", Value = "Первичный прием", Price = "2 500 - 3 000" },
                            },

                            Specification = "1".PayValue("Гастроэнтеролог"),

                            Rooms = new List<PayRoom>
                            {
                                new PayRoom
                                {
                                    Value = "каб.505 эт.2",
                                    Times = new List<PayIdValue>
                                    {
                                        "Пн-Пт".PayValue("08:00-12:48")
                                    }
                                }
                            },


                        }
                    }
                },
                new PayDoctor
                {
                    Id = "3", Value = "Вериго Андрей Валерьевич",
                    About =
@"<div><p>Заведующий офтальмологическим отделением лазерной микрохирургии стационара дневного пребывания.</p><p>Доцент кафедры офтальмологии им. академика А.П. Нестерова лечебного факультета РНИМУ им. Н.И. Пирогова.</p><p>Исполнительный директор Российского глаукомного общества.</p><p>На базе отделения работает Московский глаукомный центр.</p></div>",
                    JobAge = 12,
                    Specialities = new List<PayIdValue> { "1".PayValue("Стоматолог") , "2".PayValue("Стоматолог-терапевт") },
                    Ranges = new List<string> { "Высшая категория" },
                    LpuList = new List<PayLpuLocation>
                    {
                        new PayLpuLocation
                        {
                            Address = new PayIdValue { Value= "111539, Москва, ул. Вешняковская, 23" },
                            Lpu = new PayIdValue { Id= "GKB15", Value = "Городская клиническая больница №15 имени О. М. Филатова "},

                            Services = new List<PayServiceItem>
                            {
                                new PayServiceItem { Id = "2", Value = "Лечение кариеса", Price = "500" },
                            },

                            Specification = "1".PayValue("Стоматолог"),

                            Rooms = new List<PayRoom>
                            {
                                new PayRoom
                                {
                                    Value = "каб.505 эт.2",
                                    Times = new List<PayIdValue>
                                    {
                                        "Пн, Вт, Ср".PayValue("08:00-12:48"),
                                        "Сб".PayValue("08:00-09:00")
                                    }
                                }
                            },


                        },
                        new PayLpuLocation
                        {
                            Address = new PayIdValue { Value= "111539, Москва, ул. Вешняковская, 23" },
                            Lpu = new PayIdValue { Value = "Городская клиническая больница №15 имени О. М. Филатова "},

                            Services = new List<PayServiceItem>
                            {
                                new PayServiceItem { Id = "2", Value = "Лечение кариеса", Price = "500" },
                            },

                            Specification = "1".PayValue("Стоматолог"),

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
                            },

                        }
                    }
                },
                new PayDoctor
                {
                    Id = "4", Value = "Гончаренко Оксана Ивановна",
                    About = null,
                    //JobAge = 29,
                    Specialities = new List<PayIdValue> { "1".PayValue("Уролог")  },
                    //Ranges = new List<string> { "Высшая категория" },
                    LpuList = new List<PayLpuLocation>
                    {
                        new PayLpuLocation
                        {
                            Lpu = new PayIdValue { Id= "GKB15", Value = "Городская клиническая больница №15 имени О. М. Филатова "},

                            Services = new List<PayServiceItem>
                            {
                                new PayServiceItem { Id = "3", Value = "Look at Me Now", Price = "3 120" },
                            },

                            Specification = "1".PayValue("Уролог")

                        }
                    }
                },
                new PayDoctor
                {
                    Id = "5", Value = "Дзгоева Зарема Урузмаговна",
About =
@"<p class='hidden-sm hidden-xs'>
<h5>О приёме</h5>
Артроскопические операции, лечение заболеваний и травм костей и суставов.
</p>",
                    JobAge = 5,
                    Specialities = new List<PayIdValue> { "1".PayValue("Ортопед") , "2".PayValue("Хирург"), "3".PayValue("Травматолог") },
                    Ranges = new List<string> { "Высшая категория" },
                    LpuList = new List<PayLpuLocation>
                    {
                        new PayLpuLocation
                        {
                            Address = new PayIdValue { Value= "111539, Москва, ул. Вешняковская, 23" },
                            Lpu = new PayIdValue { Id= "GKB15", Value = "Городская клиническая больница №15 имени О. М. Филатова "},

                            Services = new List<PayServiceItem>
                            {
                                new PayServiceItem { Id = "1", Value = "приём", Price = "2 500 - 3 000" },
                            },

                            Specification = "1".PayValue("Ортопед"),

                            Rooms = new List<PayRoom>
                            {
                                new PayRoom
                                {
                                    Value = "каб.505 эт.2",
                                    Times = new List<PayIdValue>
                                    {
                                        "01.08, 02.08".PayValue("08:00-12:48"),
                                        "14.08".PayValue("08:00-13:00"),
                                    }
                                }
                            },

                        }
                    }
                }

            };
        }
        #endregion


        public async Task<List<PayIdValue>> GetSpecialityList(PayCriteria criteria)
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(1000);
                return Data.SpecList;
            });
        }

        public async Task<List<PayDoctor>> GetDoctorList(PayGetDoctorList criteria)
        {
            return await Task.Run(async () =>
            {
                //await Task.Delay(2000);
                return await Task.FromResult(Data.DocList);
            });

        }


        public async Task<List<PayVisitDay>> GetDoctorVisitDays(PayGetDoctorVisitDays criteria)
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(1000);

                DateTime d;

                if (criteria != null && DateTime.TryParseExact(criteria.DateBeg, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                {
                    DateTime.TryParseExact(criteria.DateBeg, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
                }
                else
                {
                    d = DateTime.Today;
                }

                var r = new Random(5);

                return new List<PayVisitDay>
                {
                    new PayVisitDay { Id = "1", Value = d.ToString("yyyy-MM-dd"), Text = "09:00-18:00", TicketCount = 1 },
                    new PayVisitDay { Id = "2", Value = d.AddDays(r.Next(0, 15)).ToString("yyyy-MM-dd"), Text = "09:00-18:00", TicketCount = 1},
                    new PayVisitDay { Id = "3", Value = d.AddDays(r.Next(0, 15)).ToString("yyyy-MM-dd"), Text = "12:00-13:00", TicketCount = 1},
                    new PayVisitDay { Id = "4", Value = d.AddDays(r.Next(0, 15)).ToString("yyyy-MM-dd"), Text = "10:00-19:00", TicketCount = 1}
                };

            });
        }

        public async Task<List<PaySlot>> GetDoctorVisitSlots(PayGetDoctorVisitSlots criteria)
        {
            return await Task.Run(async () =>
            {
                await Task.Delay(1000);

                return new List<PaySlot>
                {
                    new PaySlot { Id="1", Value="10:10"},
                    new PaySlot { Id="2", Value="10:20"},
                    new PaySlot { Id="3", Value="10:30"},
                    new PaySlot { Id="4", Value="10:40"},
                    new PaySlot { Id="5", Value="11:30"},
                    new PaySlot { Id="6", Value="12:43"},
                    new PaySlot { Id="7", Value="17:20"}
                };
            });
        }

        public async Task<PayDoctor> GetDoctor(PayGetDoctor criteria)
        {
            var l = await GetDoctorList(new PayGetDoctorList { });
            return l.First();
        }

        public Task<PayOrderResult> CreateDoctorVisit(PayCreateDoctorVisit criteria)
        {
            return Task.FromResult(new PayOrderResult { Id = "23", Value = "ОК!" });
        }

        public Task<PayIdValue> DeleteDoctorVisit(PayCriteria criteria)
        {
            throw new NotImplementedException();
        }


        public Task<List<PayOrderInfo>> GetDoctorVisits(PayGetDoctorVisits criteria)
        {
            throw new NotImplementedException();
        }
    }
}
