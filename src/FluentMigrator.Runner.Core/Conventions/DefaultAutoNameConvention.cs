﻿#region License
// Copyright (c) 2018, FluentMigrator Project
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Linq;

using FluentMigrator.Expressions;

namespace FluentMigrator.Runner.Conventions
{
    public class DefaultAutoNameConvention : IAutoNameConvention
    {
        public IAutoNameExpression Apply(IAutoNameExpression expression)
        {
            if (expression.AutoNameContext != AutoNameContext.EmbeddedResource)
                return expression;

            if (expression.Direction == MigrationDirection.Up)
            {
                expression.AutoName = GetAutoScriptUpNameImpl(expression.MigrationType, expression.DatabaseName);
            }
            else
            {
                expression.AutoName = GetAutoScriptDownNameImpl(expression.MigrationType, expression.DatabaseName);
            }

            return expression;
        }

        private static string GetAutoScriptUpNameImpl(Type type, string databaseType)
        {
            var migrationAttribute = type
                .GetCustomAttributes(typeof(MigrationAttribute), false)
                .OfType<MigrationAttribute>()
                .FirstOrDefault();
            if (migrationAttribute != null)
            {
                var version = migrationAttribute.Version;
                return string.Format("Scripts.Up.{0}_{1}_{2}.sql"
                    , version
                    , type.Name
                    , databaseType);
            }
            return string.Empty;
        }

        private static string GetAutoScriptDownNameImpl(Type type, string databaseType)
        {
            var migrationAttribute = type
                .GetCustomAttributes(typeof(MigrationAttribute), false)
                .OfType<MigrationAttribute>()
                .FirstOrDefault();
            if (migrationAttribute != null)
            {
                var version = migrationAttribute.Version;
                return string.Format("Scripts.Down.{0}_{1}_{2}.sql"
                    , version
                    , type.Name
                    , databaseType);
            }
            return string.Empty;
        }
    }
}
