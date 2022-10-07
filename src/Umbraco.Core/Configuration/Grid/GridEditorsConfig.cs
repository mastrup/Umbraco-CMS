using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Web.Common.DependencyInjection;
using Umbraco.Extensions;
using IHostingEnvironment = Umbraco.Cms.Core.Hosting.IHostingEnvironment;

namespace Umbraco.Cms.Core.Configuration.Grid;

internal class GridEditorsConfig : IGridEditorsConfig
{
    private readonly AppCaches _appCaches;
    private readonly IHostingEnvironment _hostingEnvironment;

    private readonly IJsonSerializer _jsonSerializer;
    private readonly ILogger<GridEditorsConfig> _logger;
    private readonly IGridEditorsConfigFileProviderFactory _gridEditorsConfigFileProviderFactory;
    private readonly IManifestParser _manifestParser;

    public GridEditorsConfig(
        AppCaches appCaches,
        IHostingEnvironment hostingEnvironment,
        IManifestParser manifestParser,
        IJsonSerializer jsonSerializer,
        ILogger<GridEditorsConfig> logger,
        IGridEditorsConfigFileProviderFactory gridEditorsConfigFileProviderFactory)
    {
        _appCaches = appCaches;
        _hostingEnvironment = hostingEnvironment;
        _manifestParser = manifestParser;
        _jsonSerializer = jsonSerializer;
        _logger = logger;
        _gridEditorsConfigFileProviderFactory = gridEditorsConfigFileProviderFactory;
    }

    [Obsolete("Use other ctor - Will be removed in Umbraco 13")]
    public GridEditorsConfig(
        AppCaches appCaches,
        IHostingEnvironment hostingEnvironment,
        IManifestParser manifestParser,
        IJsonSerializer jsonSerializer,
        ILogger<GridEditorsConfig> logger)
        : this(
              appCaches,
              hostingEnvironment,
              manifestParser,
              jsonSerializer,
              logger,
              StaticServiceProvider.Instance.GetRequiredService<IGridEditorsConfigFileProviderFactory>())
    {
    }

    public IEnumerable<IGridEditorConfig> Editors
    {
        get
        {
            List<IGridEditorConfig> GetResult()
            {
                var editors = new List<IGridEditorConfig>();

                IFileProvider? gridEditorsConfigFileProvider = _gridEditorsConfigFileProviderFactory.Create();

                if (gridEditorsConfigFileProvider is null)
                {
                    throw new ArgumentNullException(nameof(gridEditorsConfigFileProvider));
                }

                var configPath = Constants.SystemDirectories.Config.TrimStart(Constants.CharArrays.Tilde);
                IEnumerable<IFileInfo> configFiles = GetConfigFiles(gridEditorsConfigFileProvider, configPath);
                IFileInfo? gridConfig = configFiles.FirstOrDefault();

                if (gridConfig is not null)
                {
                    using Stream stream = gridConfig.CreateReadStream();
                    using var reader = new StreamReader(stream, Encoding.UTF8);
                    var sourceString = reader.ReadToEnd();

                    try
                    {
                        editors.AddRange(_jsonSerializer.Deserialize<IEnumerable<GridEditor>>(sourceString)!);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "Could not parse the contents of grid.editors.config.js into a JSON array '{Json}",
                            sourceString);
                    }
                }

                // Read default from embedded file
                else
                {
                    IFileProvider configFileProvider = new EmbeddedFileProvider(GetType().Assembly, "Umbraco.Cms.Core.EmbeddedResources.Grid");
                    IFileInfo embeddedConfig = configFileProvider.GetDirectoryContents(string.Empty)
                                    .Where(x => !x.IsDirectory && x.Name.InvariantEquals("grid.editors.config.js")).First();

                    using Stream stream = embeddedConfig.CreateReadStream();
                    using var reader = new StreamReader(stream, Encoding.UTF8);
                    var sourceString = reader.ReadToEnd();
                    editors.AddRange(_jsonSerializer.Deserialize<IEnumerable<GridEditor>>(sourceString)!);
                }

                // add manifest editors, skip duplicates
                foreach (GridEditor gridEditor in _manifestParser.CombinedManifest.GridEditors)
                {
                    if (editors.Contains(gridEditor) == false)
                    {
                        editors.Add(gridEditor);
                    }
                }

                return editors;
            }

            // cache the result if debugging is disabled
            List<IGridEditorConfig>? result = _hostingEnvironment.IsDebugMode
                ? GetResult()
                : _appCaches.RuntimeCache.GetCacheItem(typeof(GridEditorsConfig) + ".Editors", GetResult, TimeSpan.FromMinutes(10));

            return result!;
        }
    }

    private IEnumerable<IFileInfo> GetConfigFiles(IFileProvider fileProvider, string path)
    {
        IEnumerable<IFileInfo> contents = fileProvider.GetDirectoryContents(path);

        foreach (IFileInfo file in contents)
        {
            if (file.Name.InvariantEquals("grid.editors.config.js") && file.PhysicalPath != null)
            {
                yield return file;
            }
        }
    }
}
