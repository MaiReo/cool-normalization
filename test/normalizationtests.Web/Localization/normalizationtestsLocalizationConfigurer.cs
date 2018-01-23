using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;
using Abp.Reflection.Extensions;

namespace normalizationtests.Localization
{
    public static class normalizationtestsLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flags england", isDefault: true));

            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(normalizationtestsConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        typeof(normalizationtestsLocalizationConfigurer).GetAssembly(),
                        "normalizationtests.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}