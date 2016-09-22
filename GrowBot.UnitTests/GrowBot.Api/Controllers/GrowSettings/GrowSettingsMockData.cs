using System;
using System.Collections.Generic;
using GrowBot.API.Entities.GrowSettings;

namespace GrowBot.UnitTests.GrowBot.Api.Controllers.GrowSettings
{
    internal class GrowSettingsMockData
    {
        public readonly Guid _userGuid1 = new Guid("11111111-1111-1111-1111-111111111111");
        public readonly Guid _userGuid2 = new Guid("22222222-2222-2222-2222-222222222222");
        public readonly Guid _userGuid3 = new Guid("33333333-3333-3333-3333-333333333333");
        public readonly Guid _userGuid4 = new Guid("44444444-4444-4444-4444-444444444444");

        public List<GrowSetting> GetMockUserGrowData()
        {
            return GetUserGrows();
        }

        private List<GrowSetting> GetUserGrows()
        {
            return new List<GrowSetting>
            {
                #region User 1 Grow 1
                new GrowSetting
                {
                    GrowSettingId = 1,
                    UserGuid = _userGuid1,
                    CreateDateTime = DateTime.Today,
                    GrowSettingName = "General Chilli Grow",
                    SystemDefaultGrowSetting = true,
                    GrowSettingNotes = "This will grow chillis in the optimal conditions so that we get the best yield",
                    GrowPhaseSetting = new List<GrowPhaseSetting>
                    {
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 1,
                            GrowPhaseName = "Seed to sprout",
                            Duration = 10,
                            PhaseOrder = 1,
                            PhaseNotes =
                                "During this stage we want the seed to be exposed to optimal growing conditions of spring time to trigger it to begin sprouting.",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 1,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
                                },
                                new LightSetting
                                {
                                    LightSettingId = 2,
                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 1,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 2,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 3,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 4,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 1,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        },
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 2,
                            GrowPhaseName = "Vegetative Stage",
                            Duration = 30,
                            PhaseOrder = 2,
                            PhaseNotes =
                                "Now are seed has sprouted and gained its first 'true leaves'. We can begin the vegetative stage. This is where the plant will gain size. The longer it is placed in this stage, the bigger the plant and the higher the final yield will be. the usual time in this stage is 30 days. ",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 3,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
                                },
                                new LightSetting
                                {
                                    LightSettingId = 4,
                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 5,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 6,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 7,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 8,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 2,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        },
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 3,
                            GrowPhaseName = "Flowering Stage",
                            Duration = 60,
                            PhaseOrder = 3,
                            PhaseNotes =
                                "Are plant has gained size and mass in the last phase. In this phase we will begin the process of flowering. This will trigger the production of produce that we wish to harvest. This phase can usually lasts 60 days.",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 5,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("18:00", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 9,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 10,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 11,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 12,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 3,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        }
                    }
                },

                #endregion
                #region User 1 Grow 2
                new GrowSetting
                {
                    GrowSettingId = 2,
                    UserGuid = _userGuid1,
                    CreateDateTime = DateTime.Today,
                    GrowSettingName = "Strawberry Grow",
                    SystemDefaultGrowSetting = true,
                    GrowSettingNotes = "This will grow Strawberrys in the optimal conditions so that we get the best yield",
                    GrowPhaseSetting = new List<GrowPhaseSetting>
                    {
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 4,
                            GrowPhaseName = "Seed to sprout",
                            Duration = 10,
                            PhaseOrder = 1,
                            PhaseNotes = "Getting the seed to pop and show it little head",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 6,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 13,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 14,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 15,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 16,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 4,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        },
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 5,
                            GrowPhaseName = "Vegetative Stage",
                            Duration = 30,
                            PhaseOrder = 2,
                            PhaseNotes =
                                "Now are seed has sprouted and gained its first 'true leaves'. We can begin the vegetative stage. This is where the plant will gain size. The longer it is placed in this stage, the bigger the plant and the higher the final yield will be. the usual time in this stage is 30 days. ",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 7,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
                                },
                                new LightSetting
                                {
                                    LightSettingId = 8,
                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 17,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 18,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 19,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 20,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 5,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        },
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 6,
                            GrowPhaseName = "Flowering Stage",
                            Duration = 60,
                            PhaseOrder = 3,
                            PhaseNotes =
                                "Are plant has gained size and mass in the last phase. In this phase we will begin the process of flowering. This will trigger the production of produce that we wish to harvest. This phase can usually lasts 60 days.",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 9,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("18:00", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 21,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 22,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 23,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 24,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 6,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        }
                    }
                },

                #endregion

                #region User 2 Grow 1
                new GrowSetting
                {
                    GrowSettingId = 3,
                    UserGuid = _userGuid2,
                    CreateDateTime = DateTime.Today,
                    GrowSettingName = "Cress Grow",
                    SystemDefaultGrowSetting = false,
                    GrowSettingNotes = "This will grow Cress in the optimal conditions so that we get the best yield",
                    GrowPhaseSetting = new List<GrowPhaseSetting>
                    {
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 7,
                            GrowPhaseName = "Seed to sprout",
                            Duration = 10,
                            PhaseOrder = 1,
                            PhaseNotes = "Getting the seed to pop and show it little head",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 10,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 25,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 26,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 27,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 28,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 7,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        },
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 8,
                            GrowPhaseName = "Vegetative Stage",
                            Duration = 30,
                            PhaseOrder = 2,
                            PhaseNotes =
                                "Now are seed has sprouted and gained its first 'true leaves'. We can begin the vegetative stage. This is where the plant will gain size. The longer it is placed in this stage, the bigger the plant and the higher the final yield will be. the usual time in this stage is 30 days. ",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 11,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
                                },
                                new LightSetting
                                {
                                    LightSettingId = 12,
                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 29,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 30,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 31,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 32,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 7,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        },
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 9,
                            GrowPhaseName = "Flowering Stage",
                            Duration = 60,
                            PhaseOrder = 3,
                            PhaseNotes =
                                "Are plant has gained size and mass in the last phase. In this phase we will begin the process of flowering. This will trigger the production of produce that we wish to harvest. This phase can usually lasts 60 days.",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 13,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("18:00", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 33,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 34,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 35,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 36,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 8,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        }
                    }
                },

                #endregion
                #region User 2 Grow 2
                new GrowSetting
                {
                    GrowSettingId = 4,
                    UserGuid = _userGuid2,
                    CreateDateTime = DateTime.Today,
                    GrowSettingName = "Mushrooms Grow",
                    SystemDefaultGrowSetting = false,
                    GrowSettingNotes = "This will grow Mushrooms in the optimal conditions so that we get the best yield",
                    GrowPhaseSetting = new List<GrowPhaseSetting>
                    {
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 10,
                            GrowPhaseName = "Seed to sprout",
                            Duration = 10,
                            PhaseOrder = 1,
                            PhaseNotes = "Getting the seed to pop and show it little head",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 14,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 37,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 38,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 39,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 40,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 9,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        },
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 11,
                            GrowPhaseName = "Vegetative Stage",
                            Duration = 30,
                            PhaseOrder = 2,
                            PhaseNotes =
                                "Now are seed has sprouted and gained its first 'true leaves'. We can begin the vegetative stage. This is where the plant will gain size. The longer it is placed in this stage, the bigger the plant and the higher the final yield will be. the usual time in this stage is 30 days. ",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 15,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
                                },
                                new LightSetting
                                {
                                    LightSettingId = 16,
                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 41,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 42,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 43,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 44,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 10,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        },
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 12,
                            GrowPhaseName = "Flowering Stage",
                            Duration = 60,
                            PhaseOrder = 3,
                            PhaseNotes =
                                "Are plant has gained size and mass in the last phase. In this phase we will begin the process of flowering. This will trigger the production of produce that we wish to harvest. This phase can usually lasts 60 days.",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 17,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("18:00", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 45,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 46,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 47,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 48,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 11,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        }
                    }
                },

                #endregion

                #region User 3 Grow 1
                new GrowSetting
                {
                    GrowSettingId = 5,
                    UserGuid = _userGuid3,
                    CreateDateTime = DateTime.Today,
                    GrowSettingName = "Cucumber Grow",
                    SystemDefaultGrowSetting = false,
                    GrowSettingNotes = "This will grow Cucumber in the optimal conditions so that we get the best yield",
                    GrowPhaseSetting = new List<GrowPhaseSetting>
                    {
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 13,
                            GrowPhaseName = "Seed to sprout",
                            Duration = 10,
                            PhaseOrder = 1,
                            PhaseNotes = "Getting the seed to pop and show it little head",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 18,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 49,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 50,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 51,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 52,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 12,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        },
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 14,
                            GrowPhaseName = "Vegetative Stage",
                            Duration = 30,
                            PhaseOrder = 2,
                            PhaseNotes =
                                "Now are seed has sprouted and gained its first 'true leaves'. We can begin the vegetative stage. This is where the plant will gain size. The longer it is placed in this stage, the bigger the plant and the higher the final yield will be. the usual time in this stage is 30 days. ",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 19,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
                                },
                                new LightSetting
                                {
                                    LightSettingId = 20,
                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 53,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 54,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 55,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 56,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 13,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        },
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 15,
                            GrowPhaseName = "Flowering Stage",
                            Duration = 60,
                            PhaseOrder = 3,
                            PhaseNotes =
                                "Are plant has gained size and mass in the last phase. In this phase we will begin the process of flowering. This will trigger the production of produce that we wish to harvest. This phase can usually lasts 60 days.",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 21,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("18:00", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 57,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 58,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 59,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 60,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 14,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        }
                    }
                },

                #endregion
                #region User 3 Grow 2
                new GrowSetting
                {
                    GrowSettingId = 6,
                    UserGuid = _userGuid3,
                    CreateDateTime = DateTime.Today,
                    GrowSettingName = "Lettuces Grow",
                    SystemDefaultGrowSetting = false,
                    GrowSettingNotes = "This will grow Lettuces in the optimal conditions so that we get the best yield",
                    GrowPhaseSetting = new List<GrowPhaseSetting>
                    {
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 16,
                            GrowPhaseName = "Seed to sprout",
                            Duration = 10,
                            PhaseOrder = 1,
                            PhaseNotes = "Getting the seed to pop and show it little head",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 22,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 61,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 62,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 63,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 64,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 15,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        },
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 17,
                            GrowPhaseName = "Vegetative Stage",
                            Duration = 30,
                            PhaseOrder = 2,
                            PhaseNotes =
                                "Now are seed has sprouted and gained its first 'true leaves'. We can begin the vegetative stage. This is where the plant will gain size. The longer it is placed in this stage, the bigger the plant and the higher the final yield will be. the usual time in this stage is 30 days. ",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 23,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
                                },
                                new LightSetting
                                {
                                    LightSettingId = 24,
                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 65,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 66,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 67,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 68,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 16,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        },
                        new GrowPhaseSetting
                        {
                            GrowPhaseSettingId = 18,
                            GrowPhaseName = "Flowering Stage",
                            Duration = 60,
                            PhaseOrder = 3,
                            PhaseNotes =
                                "Are plant has gained size and mass in the last phase. In this phase we will begin the process of flowering. This will trigger the production of produce that we wish to harvest. This phase can usually lasts 60 days.",
                            LightSetting = new List<LightSetting>
                            {
                                new LightSetting
                                {
                                    LightSettingId = 25,
                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffLightTime = TimeSpan.ParseExact("18:00", "hh':'mm", null)
                                }
                            },
                            WaterSetting = new List<WaterSetting>
                            {
                                new WaterSetting
                                {
                                    WaterSettingId = 69,
                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 70,
                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 71,
                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
                                },
                                new WaterSetting
                                {
                                    WaterSettingId = 72,
                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
                                }
                            },
                            FanSetting = new List<FanSetting>
                            {
                                new FanSetting
                                {
                                    FanSettingId = 17,
                                    MaxHeatCelsius = 10,
                                    MinFanSpeedPercent = 25,
                                    MinHeatCelsius = 10,
                                }
                            }
                        }
                    }
                }
                #endregion
            };
        }
    }
}