// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Dolittle.Vanir.CLI.IO
{
    public interface IFileSystem
    {
        bool Exists(string path);
        string ReadFile(string path);
    }
}
