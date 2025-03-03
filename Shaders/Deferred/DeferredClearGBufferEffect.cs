#region License
//   Copyright 2014-2016 Kastellanos Nikolaos
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
#endregion

using System;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace nkast.Aether.Shaders
{
    public class DeferredClearGBufferEffect : Effect
    {
        #region Effect Parameters

        #endregion

        #region Fields


        static readonly string ResourceName = "nkast.Aether.Shaders.Resources.DeferredClearGBuffer";

        internal static byte[] LoadEffectResource(GraphicsDevice graphicsDevice, string name)
        {
            name = GetResourceName(graphicsDevice, name);
            using (Stream stream = typeof(DeferredClearGBufferEffect).Assembly.GetManifestResourceStream(name))
            {
                byte[] bytecode = new byte[stream.Length];
                stream.Read(bytecode, 0, (int)stream.Length);
                return bytecode;
            }
        }

        private static string GetResourceName(GraphicsDevice graphicsDevice, string name)
        {
            string platformName = "";
            string version = "";

#if XNA
            platformName = ".xna.WinReach";
#else
            switch (graphicsDevice.Adapter.Backend)
            {
                case GraphicsBackend.DirectX11:
                    platformName = ".dx11.fxo";
                    break;
                case GraphicsBackend.OpenGL:
                case GraphicsBackend.GLES:
                case GraphicsBackend.WebGL:
                    platformName = ".ogl.fxo";
                    break;
                default:
                    throw new NotSupportedException("platform");
            }

            // Detect version
            version = ".10";
            Version kniVersion = typeof(Effect).Assembly.GetName().Version;
            if (kniVersion.Major == 3)
            {
                if (kniVersion.Minor == 9)
                {
                    version = ".10";
                }
                if (kniVersion.Minor == 10)
                {
                    version = ".10";
                }
            }
#endif

            return name + platformName + version;
        }

        #endregion
        
        #region Public Properties
        
        #endregion

        #region Methods

         public DeferredClearGBufferEffect(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, LoadEffectResource(graphicsDevice, ResourceName))
        {    
            CacheEffectParameters(null);
        }

        public DeferredClearGBufferEffect(GraphicsDevice graphicsDevice, byte[] Bytecode): base(graphicsDevice, Bytecode)
        {   
            CacheEffectParameters(null);
        }

        protected DeferredClearGBufferEffect(DeferredClearGBufferEffect cloneSource)
            : base(cloneSource)
        {
            CacheEffectParameters(cloneSource);
        }
        
        public override Effect Clone()
        {
            return new DeferredClearGBufferEffect(this);
        }

        void CacheEffectParameters(DeferredClearGBufferEffect cloneSource)
        {    
        }
        
        #endregion
    }
}
