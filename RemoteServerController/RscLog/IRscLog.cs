﻿/******************************************************************************
 * Remote Server Controller                                                   *
 * https://github.com/prajs-org/remote-server-controller                      *
 * Copyright (C) 2014-217 Karel Prajs, karel@prajs.org                        *
 *                                                                            *
 * This program is free software: you can redistribute it and/or modify       *
 * it under the terms of the GNU General Public License as published by       *
 * the Free Software Foundation, either version 3 of the License, or          *
 * (at your option) any later version.                                        *
 *                                                                            *
 * This program is distributed in the hope that it will be useful,            *
 * but WITHOUT ANY WARRANTY; without even the implied warranty of             *
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the              *
 * GNU General Public License for more details.                               *
 *                                                                            *
 * You should have received a copy of the GNU General Public License          *
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.      *
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RscLog
{
    internal interface IRscLog
    {
        // Standard logs
        void Debug(string message, params object[] args);
        void Info(string message, params object[] args);
        void Alert(string message, params object[] args);
        void Error(string message, params object[] args);
        // Audit logs
        void AuditIncoming(string apiKey, string message, params object[] args);
        void AuditSuccess(string apiKey, string message, params object[] args);
        void AuditFailed(string apiKey, string message, params object[] args);
        // Exception logs
        void Debug(Exception exception, string comment);
        void Info(Exception exception, string comment);
        void Alert(Exception exception, string comment);
        void Error(Exception exception, string comment);
    }
}
