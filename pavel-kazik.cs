using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.ApplicationPages.Calendar.Exchange;
using Microsoft.SharePoint.Publishing;
using SkodaAuto.Dmp.DataAccess.Definitions;
using SkodaAuto.Dmp.DataAccess.Entities.ImporterWeb;
using SkodaAuto.Dmp.DataAccess.Entities.RootWeb;
using SkodaAuto.Dmp.DataAccess.Helpers;
using SkodaAuto.Dmp.DataAccess.Repositories.Base;
using SkodaAuto.Dmp.DataAccess.Repositories.RootWeb;
using SkodaAuto.Dmp.DataAccess.Security;
using SkodaAuto.Dmp.Impl.Managers.Base;

namespace SkodaAuto.Dmp.Impl.Managers
{
    public class ImporterDefinitionManager : ManagerBase
    {
        private readonly ImporterInfoRepository infoRepository;
        
        private readonly ImporterContentDefinitionRepository contentRepository;

        public ImporterDefinitionManager(SPWeb rootWeb) : base(rootWeb)
        {
            infoRepository = new ImporterInfoRepository(RootWeb);
            contentRepository = new ImporterContentDefinitionRepository(RootWeb);
        }

        public Importer GetImporter(int id)
        {
            var importerInfo = infoRepository.GetById(id);
            return LoadAdditionalInformation(importerInfo);
        }

        public Importer GetImporter(string shortcut)
        {
            var importerInfo = infoRepository.GetByBID(shortcut);
            return LoadAdditionalInformation(importerInfo);
        }

        public Importer GetImporterByWebUrl(string webUrl)
        {
            var importerInfo = infoRepository.GetByImporterWebUrl(webUrl);
            return LoadAdditionalInformation(importerInfo);
        }

        /// <summary>
        /// Get all importes
        /// </summary>
        /// <param name="isActive">TRUE - return active | FALSE - return not active | NULL - return all</param>
        /// <returns></returns>
        public List<Importer> GetAllImporters(bool? isActive = true)
        {
            var importerInfos = infoRepository.GetActive(isActive);

            var importers = new List<Importer>();

            foreach (var importerInfo in importerInfos)
                importers.Add(LoadAdditionalInformation(importerInfo));

            return importers;
        }

        public List<ImporterInfo> GetActiveImporterInfosWithWeb()
        {
            var importerInfos = infoRepository.GetActive(true);
            return importerInfos.Where(info => !string.IsNullOrEmpty(info.ImporterWebUrl)).OrderBy(info => info.Title).ToList();
        }

        /// <summary>
        /// Get only importer infos, withuot Access and Content definitions
        /// </summary>
        /// <param name="isActive">TRUE - return active | FALSE - return not active | NULL - return all</param>
        /// <returns></returns>
        public List<ImporterInfo> GetInfoForAllImporters(bool? isActive = true)
        {
            return infoRepository.GetActive(isActive);
        }

        public List<ImporterInfo> GetInfoForUser(B2BUser user)
        {
            return user.BIDs == null ? null : user.BIDs.Select(bid => infoRepository.GetByBID(bid)).ToList();
        }

        private Importer LoadAdditionalInformation(ImporterInfo importerInfo)
        {
            if (importerInfo == null) throw new Exception("Importer does not found");

            var contentDefinitions = contentRepository.GetForImporter(importerInfo);

            return new Importer(importerInfo, contentDefinitions);
        }

        public void Save(Importer importer)
        {
            infoRepository.Save(importer.Info);

            importer.ContentDefinition.Title = importer.Info.Title;
            importer.ContentDefinition.Importer = importer.Info.GetAsLookup();

            contentRepository.Save(importer.ContentDefinition);
        }

        public void Delete(Importer importer)
        {
            if (!string.IsNullOrEmpty(importer.Info.ImporterWebUrl) || RepositoryHelper.ExistSPWeb(RootWeb, importer.Info.BID.ToLower()))
                DeleteImporterWebAndRoles(RootWeb, importer);

            infoRepository.Delete(importer.Info.Id);

            if (importer.ContentDefinition != null)
                contentRepository.Delete(importer.ContentDefinition.Id);
        }

        public void ChangeImporterWebTitle(string importerWebUrl, string newTitle)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (var site = new SPSite(importerWebUrl))
                using (var web = site.OpenWeb())
                {
                    var defaultUpdates = web.AllowUnsafeUpdates;
                    web.AllowUnsafeUpdates = true;

                    web.Title = newTitle;
                    web.Description = "Digital Marketing Plan - " + newTitle;
                    web.Update();

                    web.AllowUnsafeUpdates = defaultUpdates;
                }
            });
        }

        private void DeleteImporterWebAndRoles(SPWeb rootWeb, Importer importer)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                var webUrl = rootWeb + importer.Info.ImporterWebUrl;

                using (var site = new SPSite(webUrl))
                {
                    // Delete Importer web
                    using (var web = site.OpenWeb())
                        web.Delete();

                    // Delete Importer groups on root web
                    foreach (var availableRole in AccessRolesMapper.GetAllAvailableRoles())
                    {
                        var groupName = SecurityHelper.GetGroupName(importer.Info.BID, AccessRolesMapper.GetRoleName(availableRole.Key));
                        site.RootWeb.SiteGroups.Remove(groupName);
                    }

                    site.RootWeb.Update();
                }
            });
        }

        #region === Create Web ===

        public void CreateImporterWeb(Importer importer)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                using (var rootSite = new SPSite(RootWeb.Site.Url))
                {
                    // Vytvoreni webu
                    var importerWeb = PrepareImporterWebSpace(rootSite, importer);

                    var defaultUnsafeUpdates = importerWeb.AllowUnsafeUpdates;
                    importerWeb.AllowUnsafeUpdates = true;

                    // Priprava navigace
                    PrepareImporterNavigation(importerWeb);

                    // Aktivace features pro Importersky web
                    ActivationImporterWebFeatures(importerWeb);

                    // Zpropaguji se zakladni ciselnikove hodnoty do seznamu pro aktualni rok
                    PropagateCodeTableValuesToImporterWebSpace(rootSite, importerWeb);

                    // Nastavi se opravneni
                    PrepareRolesAndUsersOnImporterWeb(importerWeb, importer);

                    importerWeb.AllowUnsafeUpdates = defaultUnsafeUpdates;
                    importerWeb.Update();

                    // Nastaveni ImporterWebUrl k definici importera
                    importer.Info.ImporterWebUrl = importerWeb.ServerRelativeUrl;
                }
            });

            Save(importer);
        }

        private void PrepareImporterNavigation(SPWeb importerWeb)
        {
            var pweb = PublishingWeb.GetPublishingWeb(importerWeb);
            if (!pweb.Navigation.InheritCurrent)
            {
                pweb.Navigation.InheritCurrent = true;
                pweb.Web.Update();
            }

            importerWeb.Navigation.UseShared = true;
        }

        private void ActivationImporterWebFeatures(SPWeb importerWeb)
        {
            EnableFeature(importerWeb, FeatureDefinitions.ImporterWeb_Modules);
            EnableFeature(importerWeb, FeatureDefinitions.ImporterWeb_DelegateControls); 
            EnableFeature(importerWeb, FeatureDefinitions.ImporterWeb_Lists);
            EnableFeature(importerWeb, FeatureDefinitions.ImporterWeb_Receivers);
        }

        private static void EnableFeature(SPWeb importerWeb, Guid featureId)
        {
            if (importerWeb.Features.FirstOrDefault(feature => feature.DefinitionId == featureId) == null)
                importerWeb.Features.Add(featureId, true);
        }

        private static void PrepareRolesAndUsersOnImporterWeb(SPWeb importerWeb, Importer importer)
        {
            importerWeb.BreakRoleInheritance(true, true);

            CreateWebGroups(importerWeb, importer.Info.BID);
            GrantPermissionsToLists(importerWeb, importer.Info.BID);
        }

        private static void GrantPermissionsToLists(SPWeb importerWeb, string importerShortcut)
        {
            foreach (var accessDefinition in new ImporterYear().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.ImporterYears, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new ManagementSummary().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.ManagementSummary, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new ImporterCalendarActivities().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.ImporterCalendarActivities, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new ImporterCalendarActivityTypes().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.ImporterCalendarActivityTypes, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new ImporterCalendarCustomActivityTypes().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.ImporterCalendarCustomActivityTypes, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new MarketingBudget().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.MarketingBudget, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new KPIs().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.KPIs, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new EngagementPlatformsGroupBudget().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.EngagementPlatforms_GroupBudgets, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new EngagementPlatformsImporterGroup().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.EngagementPlatforms_ImporterGroups, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new EngagementPlatformsImporterLevel().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.EngagementPlatforms_ImporterLevels, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new EngagementPlatformsImporterQuestion().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.EngagementPlatforms_ImporterQuestions, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new EngagementPlatformsLevelDescription().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.EngagementPlatforms_LevelDescriptions, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new EngagementPlatformsQuestionValue().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.EngagementPlatforms_QuestionValues, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new SpecialNeeds().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.SpecialNeeds, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new SpecialThreats().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.SpecialThreats, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new ImporterMeasuresAndActions().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.MeasuresAndActions, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new PreliminaryMediaPlanImporterGroup().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.PreliminaryMediaPlan_ImporterGroups, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new PreliminaryMediaPlanImporterLevel().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.PreliminaryMediaPlan_ImporterLevels, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new PreliminaryMediaPlanImporterSublevel().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.PreliminaryMediaPlan_ImporterSubevels, importerShortcut, accessDefinition);

            foreach (var accessDefinition in new PreliminaryMediaPlanValue().ListAccessDefinitions())
                CreateRoleAssigmentForList(importerWeb, ListDefinitions.PreliminaryMediaPlan_Values, importerShortcut, accessDefinition);           
        }

        private static void CreateRoleAssigmentForList(SPWeb importerWeb, string listUrl, string importerShortcut, KeyValuePair<AccessRolesEnum, PermissionLevelEnum> accessDefinition)
        {
            var roleDefinition = importerWeb.RoleDefinitions[PermissionLevelsMapper.GetPermissionLevelName(accessDefinition.Value)];

            var groupName = SecurityHelper.GetGroupName(importerShortcut, AccessRolesMapper.GetRoleName(accessDefinition.Key));

            if (accessDefinition.Key == AccessRolesEnum.Administrator)
                groupName = AccessRolesMapper.RoleName_Administrator;

            var roleAssigment = new SPRoleAssignment(importerWeb.SiteGroups[groupName]);
            roleAssigment.RoleDefinitionBindings.Add(roleDefinition);

            var list = SpListHelper.GetSpList(importerWeb, listUrl);
            if (!list.HasUniqueRoleAssignments) list.BreakRoleInheritance(true, true);

            list.RoleAssignments.Add(roleAssigment);
            list.Update();
        }

        private static void CreateWebGroups(SPWeb importerWeb, string importerShortcut)
        {
            var siteAdministrators = importerWeb.SiteAdministrators.Cast<SPUser>().Where(user => !user.IsDomainGroup).ToList();
            if (siteAdministrators.Count == 0) throw new Exception("Cannot get site admin unoccupied in domain group");

            foreach (var role in AccessRolesMapper.GetAllAvailableRoles(true))
            {
                var groupName = role.Key == AccessRolesEnum.Administrator
                    ? role.Value : SecurityHelper.GetGroupName(importerShortcut, role.Value);

                if (role.Key != AccessRolesEnum.Administrator)
                    importerWeb.SiteGroups.Add(groupName, siteAdministrators[0], null, string.Empty);

                var spGroup = importerWeb.SiteGroups.Cast<SPGroup>().SingleOrDefault(group => group.Name == groupName);

                if (spGroup == null) throw new Exception(string.Format("Cannot find group: {0}", groupName));

                spGroup.OnlyAllowMembersViewMembership = false;
                spGroup.Update();

                var roleAssigment = new SPRoleAssignment(importerWeb.SiteGroups[groupName]);
                var roleDefinitions = importerWeb.RoleDefinitions.GetByType(SPRoleType.Reader);

                roleAssigment.RoleDefinitionBindings.Add(roleDefinitions);

                importerWeb.RoleAssignments.Add(roleAssigment);
            }
        }

        private static void PropagateCodeTableValuesToImporterWebSpace(SPSite rootSite, SPWeb importerWeb)
        {
            var codeTableManager = new CodeTablesManager(rootSite.RootWeb);
            var codebooksYearRepository = new CodebooksYearsRepository(rootSite.RootWeb);

            var codebooksYears = codebooksYearRepository.GetAllItems();

            foreach (var codebooksYear in codebooksYears)
            {
                if (!codebooksYear.ArePropagatedToImporters.HasValue) continue;
                if (!codebooksYear.ArePropagatedToImporters.Value) continue;
                if (DateTime.Now.Year > int.Parse(codebooksYear.Title)) continue;

                codeTableManager.PrepareValuesForYear(importerWeb, int.Parse(codebooksYear.Title));
            }
        }

        private static SPWeb PrepareImporterWebSpace(SPSite rootSite, Importer importer)
        {
            var locale = rootSite.RootWeb.Language;

            var templates = rootSite.GetWebTemplates(locale);
            var webTemplate = templates.Cast<SPWebTemplate>().FirstOrDefault(template => template.Title.Equals(ApplicationConstants.WebTemplateName));

            var importerWeb = rootSite.AllWebs.Add(importer.Info.BID.ToLower().Trim(), importer.Info.Title,
                string.Format("Digital Marketing Plan - {0}", importer.Info.Title), locale, webTemplate, true, false);

            var rootWeb = importerWeb.Site.RootWeb;

            importerWeb.ThemedCssFolderUrl = rootWeb.ThemedCssFolderUrl;
            importerWeb.CustomMasterUrl = rootWeb.CustomMasterUrl;
            importerWeb.MasterUrl = rootWeb.MasterUrl;
            importerWeb.Properties.Add(ApplicationConstants.ImporterWebPropertyKey_BID, importer.Info.BID);

            importerWeb.Update();

            return importerWeb;
        }

        #endregion
    }
}
