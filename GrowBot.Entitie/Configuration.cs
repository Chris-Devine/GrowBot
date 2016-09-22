//using System.Collections.Generic;
//using GrowBot.API.Entities.Basics;
//using GrowBot.API.Entities.GrowSettings;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;

//namespace GrowBot.API.DbContext.Migrations
//{
//    using System;
//    using System.Data.Entity;
//    using System.Data.Entity.Migrations;
//    using System.Linq;

//    internal sealed class Configuration : DbMigrationsConfiguration<GrowBot.API.DbContext.ApplicationDbContext>
//    {
//        public Configuration()
//        {
//            AutomaticMigrationsEnabled = false;
//        }

//        protected override void Seed(ApplicationDbContext context)
//        {
//            string[] roles = { "Administrator", "Grower", "User" };

//            #region Seed Users & Roles

//            add the admin section
//            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
//            var userManager = new UserManager<ApplicationUser>(userStore);
//            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

//            #region Add Roles

//             add admin role
//            if (!roleManager.RoleExists(roles[0]))
//            {
//                var role = new IdentityRole { Name = roles[0] };
//                roleManager.Create(role);
//            }
//             add product owner
//            if (!roleManager.RoleExists(roles[1]))
//            {
//                var role = new IdentityRole { Name = roles[1] };
//                roleManager.Create(role);
//            }
//             add general user
//            if (!roleManager.RoleExists(roles[2]))
//            {
//                var role = new IdentityRole { Name = roles[2] };
//                roleManager.Create(role);
//            }

//            #endregion

//            #region Add Admin

//            var adminUserDetails = new ApplicationUser
//            {
//                UserName = "chris@mrdevine.co.uk",
//                PhoneNumber = "0797697898",
//                Email = "chris@mrdevine.co.uk",
//                EmailConfirmed = true,
//                FirstName = "Chris",
//                LastName = "Devine"
//            };

//            add admin account
//            if (!(userManager.Users.Any(u => u.UserName == "chris@mrdevine.co.uk")))
//            {
//                userManager.Create(adminUserDetails, "Projectgrow123");
//            }
//            else
//            {
//                adminUserDetails = userManager.Users.FirstOrDefault(u => u.UserName == "chris@mrdevine.co.uk");
//            }

//            add admin to role admin
//            if (!userManager.IsInRole(adminUserDetails.Id, roles[0]))
//            {
//                userManager.AddToRole(adminUserDetails.Id, roles[0]);
//            }

//            #endregion

//            #region Add Product Owner 1

//            var productOwner1UserDetails = new ApplicationUser
//            {
//                UserName = "ProductOwner1@mrdevine.co.uk",
//                PhoneNumber = "0797697898",
//                Email = "ProductOwner1@mrdevine.co.uk",
//                EmailConfirmed = true,
//                FirstName = "Stacy",
//                LastName = "Brookfield"
//            };

//            add admin account
//            if (!(userManager.Users.Any(u => u.UserName == "ProductOwner1@mrdevine.co.uk")))
//            {
//                userManager.Create(productOwner1UserDetails, "Projectgrow123");
//            }
//            else
//            {
//                productOwner1UserDetails =
//                    userManager.Users.FirstOrDefault(u => u.UserName == "ProductOwner1@mrdevine.co.uk");
//            }

//            add admin to role admin
//            if (!userManager.IsInRole(productOwner1UserDetails.Id, roles[1]))
//            {
//                userManager.AddToRole(productOwner1UserDetails.Id, roles[1]);
//            }

//            #endregion

//            #region Add Product Owner 2

//            var productOwner2UserDetails = new ApplicationUser
//            {
//                UserName = "ProductOwner2@mrdevine.co.uk",
//                PhoneNumber = "0797697898",
//                Email = "ProductOwner2@mrdevine.co.uk",
//                EmailConfirmed = true,
//                FirstName = "test",
//                LastName = "er1"
//            };

//            add admin account
//            if (!(userManager.Users.Any(u => u.UserName == "ProductOwner2@mrdevine.co.uk")))
//            {
//                userManager.Create(productOwner2UserDetails, "Projectgrow123");
//            }
//            else
//            {
//                productOwner2UserDetails =
//                    userManager.Users.FirstOrDefault(u => u.UserName == "ProductOwner2@mrdevine.co.uk");
//            }

//            add admin to role admin
//            if (!userManager.IsInRole(productOwner2UserDetails.Id, roles[1]))
//            {
//                userManager.AddToRole(productOwner2UserDetails.Id, roles[1]);
//            }

//            #endregion

//            #region Add General User 1

//            var generalUser1UserDetails = new ApplicationUser
//            {
//                UserName = "GeneralUser1@mrdevine.co.uk",
//                PhoneNumber = "0797697898",
//                Email = "GeneralUser1@mrdevine.co.uk",
//                EmailConfirmed = true,
//                FirstName = "test",
//                LastName = "er2"
//            };

//            add admin account
//            if (!(userManager.Users.Any(u => u.UserName == "GeneralUser1@mrdevine.co.uk")))
//            {
//                userManager.Create(generalUser1UserDetails, "Projectgrow123");
//            }
//            else
//            {
//                generalUser1UserDetails =
//                    userManager.Users.FirstOrDefault(u => u.UserName == "GeneralUser1@mrdevine.co.uk");
//            }

//            add admin to role admin
//            if (!userManager.IsInRole(generalUser1UserDetails.Id, roles[2]))
//            {
//                userManager.AddToRole(generalUser1UserDetails.Id, roles[2]);
//            }

//            #endregion

//            #region Add General User 2

//            var generalUser2UserDetails = new ApplicationUser
//            {
//                UserName = "GeneralUser2@mrdevine.co.uk",
//                PhoneNumber = "0797697898",
//                Email = "GeneralUser2@mrdevine.co.uk",
//                EmailConfirmed = true,
//                FirstName = "test",
//                LastName = "er3"
//            };

//            add admin account
//            if (!(userManager.Users.Any(u => u.UserName == "GeneralUser2@mrdevine.co.uk")))
//            {
//                userManager.Create(generalUser2UserDetails, "Projectgrow123");
//            }
//            else
//            {
//                generalUser2UserDetails =
//                    userManager.Users.FirstOrDefault(u => u.UserName == "GeneralUser2@mrdevine.co.uk");
//            }

//            add admin to role admin
//            if (!userManager.IsInRole(generalUser2UserDetails.Id, roles[2]))
//            {
//                userManager.AddToRole(generalUser2UserDetails.Id, roles[2]);
//            }

//            #endregion

//            #endregion

//            #region Seed UserGrow Settings

//            #region Admin Grow 1

//            if (!context.UserGrow.Any(ug => ug.GrowName == "General Chilli Grow" && ug.SystemDefaultGrow))
//            {
//                context.UserGrow.Add(new UserGrow
//                {
//                    UserGuid = new Guid(adminUserDetails.Id),
//                    CreateDateTime = DateTime.Today,
//                    GrowName = "General Chilli Grow",
//                    SystemDefaultGrow = true,
//                    PublicGrow = true,
//                    GrowNotes = "This will grow chillis in the optimal conditions so that we get the best yield",
//                    GrowPhaseSetting = new List<GrowPhaseSetting>
//                    {
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Seed to sprout",
//                            Duration = 10,
//                            PhaseOrder = 1,
//                            PhaseNotes =
//                                "During this stage we want the seed to be exposed to optimal growing conditions of spring time to trigger it to begin sprouting.",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
//                                },
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        },
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Vegetative Stage",
//                            Duration = 30,
//                            PhaseOrder = 2,
//                            PhaseNotes =
//                                "Now are seed has sprouted and gained its first 'true leaves'. We can begin the vegetative stage. This is where the plant will gain size. The longer it is placed in this stage, the bigger the plant and the higher the final yield will be. the usual time in this stage is 30 days. ",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
//                                },
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        },
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Flowering Stage",
//                            Duration = 60,
//                            PhaseOrder = 3,
//                            PhaseNotes =
//                                "Are plant has gained size and mass in the last phase. In this phase we will begin the process of flowering. This will trigger the production of produce that we wish to harvest. This phase can usually lasts 60 days.",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("18:00", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        }
//                    }
//                });
//            }

//            #endregion

//            #region Admin Grow 2

//            if (!context.UserGrow.Any(ug => ug.GrowName == "Strawberry Grow" && ug.SystemDefaultGrow))
//            {
//                context.UserGrow.Add(new UserGrow
//                {
//                    UserGuid = new Guid(adminUserDetails.Id),
//                    CreateDateTime = DateTime.Today,
//                    GrowName = "Strawberry Grow",
//                    SystemDefaultGrow = true,
//                    PublicGrow = true,
//                    GrowNotes = "This will grow Strawberrys in the optimal conditions so that we get the best yield",
//                    GrowPhaseSetting = new List<GrowPhaseSetting>
//                    {
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Seed to sprout",
//                            Duration = 10,
//                            PhaseOrder = 1,
//                            PhaseNotes = "Getting the seed to pop and show it little head",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        },
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Vegetative Stage",
//                            Duration = 30,
//                            PhaseOrder = 2,
//                            PhaseNotes =
//                                "Now are seed has sprouted and gained its first 'true leaves'. We can begin the vegetative stage. This is where the plant will gain size. The longer it is placed in this stage, the bigger the plant and the higher the final yield will be. the usual time in this stage is 30 days. ",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
//                                },
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        },
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Flowering Stage",
//                            Duration = 60,
//                            PhaseOrder = 3,
//                            PhaseNotes =
//                                "Are plant has gained size and mass in the last phase. In this phase we will begin the process of flowering. This will trigger the production of produce that we wish to harvest. This phase can usually lasts 60 days.",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("18:00", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        }
//                    }
//                });
//            }

//            #endregion

//            #region Product Owner 1 Grow 1

//            if (!context.UserGrow.Any(ug => ug.GrowName == "Cress Grow" && ug.SystemDefaultGrow))
//            {
//                context.UserGrow.Add(new UserGrow
//                {
//                    UserGuid = new Guid(productOwner1UserDetails.Id),
//                    CreateDateTime = DateTime.Today,
//                    GrowName = "Cress Grow",
//                    SystemDefaultGrow = false,
//                    PublicGrow = false,
//                    GrowNotes = "This will grow Cress in the optimal conditions so that we get the best yield",
//                    GrowPhaseSetting = new List<GrowPhaseSetting>
//                    {
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Seed to sprout",
//                            Duration = 10,
//                            PhaseOrder = 1,
//                            PhaseNotes = "Getting the seed to pop and show it little head",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        },
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Vegetative Stage",
//                            Duration = 30,
//                            PhaseOrder = 2,
//                            PhaseNotes =
//                                "Now are seed has sprouted and gained its first 'true leaves'. We can begin the vegetative stage. This is where the plant will gain size. The longer it is placed in this stage, the bigger the plant and the higher the final yield will be. the usual time in this stage is 30 days. ",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
//                                },
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        },
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Flowering Stage",
//                            Duration = 60,
//                            PhaseOrder = 3,
//                            PhaseNotes =
//                                "Are plant has gained size and mass in the last phase. In this phase we will begin the process of flowering. This will trigger the production of produce that we wish to harvest. This phase can usually lasts 60 days.",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("18:00", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        }
//                    }
//                });
//            }

//            #endregion

//            #region Product Owner 1 Grow 2

//            if (!context.UserGrow.Any(ug => ug.GrowName == "Mushrooms Grow" && ug.SystemDefaultGrow))
//            {
//                context.UserGrow.Add(new UserGrow
//                {
//                    UserGuid = new Guid(productOwner1UserDetails.Id),
//                    CreateDateTime = DateTime.Today,
//                    GrowName = "Mushrooms Grow",
//                    SystemDefaultGrow = false,
//                    PublicGrow = false,
//                    GrowNotes = "This will grow Mushrooms in the optimal conditions so that we get the best yield",
//                    GrowPhaseSetting = new List<GrowPhaseSetting>
//                    {
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Seed to sprout",
//                            Duration = 10,
//                            PhaseOrder = 1,
//                            PhaseNotes = "Getting the seed to pop and show it little head",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        },
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Vegetative Stage",
//                            Duration = 30,
//                            PhaseOrder = 2,
//                            PhaseNotes =
//                                "Now are seed has sprouted and gained its first 'true leaves'. We can begin the vegetative stage. This is where the plant will gain size. The longer it is placed in this stage, the bigger the plant and the higher the final yield will be. the usual time in this stage is 30 days. ",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
//                                },
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        },
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Flowering Stage",
//                            Duration = 60,
//                            PhaseOrder = 3,
//                            PhaseNotes =
//                                "Are plant has gained size and mass in the last phase. In this phase we will begin the process of flowering. This will trigger the production of produce that we wish to harvest. This phase can usually lasts 60 days.",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("18:00", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        }
//                    }
//                });
//            }

//            #endregion

//            #region Product Owner 2 Grow 1

//            if (!context.UserGrow.Any(ug => ug.GrowName == "Cucumber Grow" && ug.SystemDefaultGrow))
//            {
//                context.UserGrow.Add(new UserGrow
//                {
//                    UserGuid = new Guid(productOwner2UserDetails.Id),
//                    CreateDateTime = DateTime.Today,
//                    GrowName = "Cucumber Grow",
//                    SystemDefaultGrow = false,
//                    PublicGrow = false,
//                    GrowNotes = "This will grow Cucumber in the optimal conditions so that we get the best yield",
//                    GrowPhaseSetting = new List<GrowPhaseSetting>
//                    {
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Seed to sprout",
//                            Duration = 10,
//                            PhaseOrder = 1,
//                            PhaseNotes = "Getting the seed to pop and show it little head",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        },
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Vegetative Stage",
//                            Duration = 30,
//                            PhaseOrder = 2,
//                            PhaseNotes =
//                                "Now are seed has sprouted and gained its first 'true leaves'. We can begin the vegetative stage. This is where the plant will gain size. The longer it is placed in this stage, the bigger the plant and the higher the final yield will be. the usual time in this stage is 30 days. ",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
//                                },
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        },
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Flowering Stage",
//                            Duration = 60,
//                            PhaseOrder = 3,
//                            PhaseNotes =
//                                "Are plant has gained size and mass in the last phase. In this phase we will begin the process of flowering. This will trigger the production of produce that we wish to harvest. This phase can usually lasts 60 days.",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("18:00", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        }
//                    }
//                });
//            }

//            #endregion

//            #region Product Owner 2 Grow 2

//            if (!context.UserGrow.Any(ug => ug.GrowName == "Lettuces Grow" && ug.SystemDefaultGrow))
//            {
//                context.UserGrow.Add(new UserGrow
//                {
//                    UserGuid = new Guid(productOwner2UserDetails.Id),
//                    CreateDateTime = DateTime.Today,
//                    GrowName = "Lettuces Grow",
//                    SystemDefaultGrow = false,
//                    PublicGrow = false,
//                    GrowNotes = "This will grow Lettuces in the optimal conditions so that we get the best yield",
//                    GrowPhaseSetting = new List<GrowPhaseSetting>
//                    {
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Seed to sprout",
//                            Duration = 10,
//                            PhaseOrder = 1,
//                            PhaseNotes = "Getting the seed to pop and show it little head",
//                            FanSetting = new List<FanSetting>
//                            {
//                                new FanSetting
//                                {
//                                    MaxHeatCelsius = 1,
//                                    MinHeatCelsius = 0,
//                                    MinFanSpeedPercent = 20
//                                }
//                            },
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        },
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Vegetative Stage",
//                            Duration = 30,
//                            PhaseOrder = 2,
//                            PhaseNotes =
//                                "Now are seed has sprouted and gained its first 'true leaves'. We can begin the vegetative stage. This is where the plant will gain size. The longer it is placed in this stage, the bigger the plant and the higher the final yield will be. the usual time in this stage is 30 days. ",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("12:59", "hh':'mm", null)
//                                },
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("13:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("23:59", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        },
//                        new GrowPhaseSetting
//                        {
//                            GrowPhaseName = "Flowering Stage",
//                            Duration = 60,
//                            PhaseOrder = 3,
//                            PhaseNotes =
//                                "Are plant has gained size and mass in the last phase. In this phase we will begin the process of flowering. This will trigger the production of produce that we wish to harvest. This phase can usually lasts 60 days.",
//                            LightSetting = new List<LightSetting>
//                            {
//                                new LightSetting
//                                {
//                                    TurnOnLightTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffLightTime = TimeSpan.ParseExact("18:00", "hh':'mm", null)
//                                }
//                            },
//                            WaterSetting = new List<WaterSetting>
//                            {
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("06:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("06:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("12:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("12:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("18:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("18:15", "hh':'mm", null)
//                                },
//                                new WaterSetting
//                                {
//                                    TurnOnWaterTime = TimeSpan.ParseExact("23:00", "hh':'mm", null),
//                                    TurnOffWaterTime = TimeSpan.ParseExact("23:15", "hh':'mm", null)
//                                }
//                            }
//                        }
//                    }
//                });
//            }

//            #endregion

//            #endregion
//        }
//    }
//}

