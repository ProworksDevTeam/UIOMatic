﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Migrations;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core;

namespace UIOMatic.Migrations
{
    public class UpgradeComponentComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<UpgradeComponent>();
        }
    }

    public class UpgradeComponent : IComponent
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IMigrationBuilder _migrationBuilder;
        private readonly IKeyValueService _keyValueService;
        private readonly ILogger _logger;

        public UpgradeComponent(IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
        {
            _scopeProvider = scopeProvider;
            _migrationBuilder = migrationBuilder;
            _keyValueService = keyValueService;
            _logger = logger;
        }
        public void Initialize()
        {
            var plan = new MigrationPlan("UIOMatic");
            plan.From(string.Empty)
                .To<AddAllowedSectionToAdmins>("state-3.0.0");

            plan.From("state-3.0.0")
                .To<InstancePing>("state-3.1.3");

            plan.From("state-3.1.3")
              .To<InstancePing>("state-3.1.4");

            var upgrader = new Upgrader(plan);
            upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);

        }

        public void Terminate()
        {
        }
    }
}
