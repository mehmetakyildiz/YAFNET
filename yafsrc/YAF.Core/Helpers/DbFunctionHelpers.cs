/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2015 Ingo Herbote
 * http://www.yetanotherforum.net/
 * 
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace YAF.Core.Helpers
{
    using System;
    using System.Linq;

    using YAF.Types;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces.Data;

    public static class DbFunctionHelper
    {
        private static T ValidateAndExecute<T>(this IDbFunction dbFunction, string operationName, Func<IDbFunction, T> func)
        {
            if (!dbFunction.DbSpecificFunctions.WhereOperationSupported(operationName).Any())
            {
                throw new InvalidOperationException(@"Database Provider does not support operation ""{0}"".".FormatWith(operationName));
            }

            return func(dbFunction);
        }

        public static int GetDBSize([NotNull] this IDbFunction dbFunction)
        {
            CodeContracts.VerifyNotNull(dbFunction, "dbFunction");

            return dbFunction.ValidateAndExecute<int>("DBSize", f => f.GetScalar<int>(s => s.DBSize()));
        }
    }
}