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

namespace tainicom.Aether.Shaders
{
    public class DeferredPointLightEffect : Effect, IEffectMatrices
    {
        #region Effect Parameters
        
        EffectParameter colorMapParam;
        EffectParameter normalMapParam;
        EffectParameter depthMapParam;
        EffectParameter halfPixelParam;

        EffectParameter lightPositionParam;
        EffectParameter colorParam;
        EffectParameter lightRadiusParam;
        EffectParameter lightIntensityParam;
        EffectParameter cameraPositionParam;
        EffectParameter invertViewProjectionParam;

        // IEffectMatrices
        EffectParameter projectionParam;
        EffectParameter viewParam;
        EffectParameter worldParam;

        #endregion

        #region Fields


#if ((MG && WINDOWS) || W10)
        static readonly String resourceName = "tainicom.Aether.Shaders.Resources.DeferredPointLight.dx11.mgfxo";
#else
        static readonly String resourceName = "tainicom.Aether.Shaders.Resources.DeferredPointLight.xna.WinReach";
#endif

        internal static byte[] LoadEffectResource(string name)
        {
            using (var stream = LoadEffectResourceStream(name))
            {
                var bytecode = new byte[stream.Length];
                stream.Read(bytecode, 0, (int)stream.Length);
                return bytecode;
            }
        }

        internal static Stream LoadEffectResourceStream(string name)
        {
            // Detect MG version            
            var version = "";
#if !XNA
            version = ".9";
            var mgVersion = GetAssembly(typeof(Effect)).GetName().Version;
            if (mgVersion.Major == 3)
            {
                if (mgVersion.Minor == 4)
                    version = ".6";
                if (mgVersion.Minor == 5)
                    version = ".7";
                if (mgVersion.Minor == 6)
                    version = ".8";
                if (mgVersion.Minor == 7)
                    version = ".8";
                if (mgVersion.Minor == 8)
                    version = ".9";
            }
            name = name + version;
#endif

            Stream stream = GetAssembly(typeof(DeferredPointLightEffect)).GetManifestResourceStream(name);
            return stream;
        }

        private static Assembly GetAssembly(Type type)
        {            
            #if W10 
            return type.GetTypeInfo().Assembly;
            #else
            return type.Assembly;
            #endif
        }

        #endregion
        
        #region Public Properties

        public RenderTarget2D ColorMap
        {
            get { return colorMapParam.GetValueTexture2D() as RenderTarget2D; }
            set { colorMapParam.SetValue(value); }
        }

        public RenderTarget2D NormalMap
        {
            get { return normalMapParam.GetValueTexture2D() as RenderTarget2D; }
            set { normalMapParam.SetValue(value); }
        }

        public RenderTarget2D DepthMap
        {
            get { return depthMapParam.GetValueTexture2D() as RenderTarget2D; }
            set { depthMapParam.SetValue(value); }
        }

        public Vector2 HalfPixel
        {
            get { return halfPixelParam.GetValueVector2(); }
            set { halfPixelParam.SetValue(value); }
        }

        public Vector3 LightPosition
        {
            get { return lightPositionParam.GetValueVector3(); }
            set { lightPositionParam.SetValue(value); }
        }

        public Vector3 Color
        {
            get { return colorParam.GetValueVector3(); }
            set { colorParam.SetValue(value); }
        }

        public float LightRadius
        {
            get { return lightRadiusParam.GetValueSingle(); }
            set { lightRadiusParam.SetValue(value); }
        }

        public float LightIntensity
        {
            get { return lightIntensityParam.GetValueSingle(); }
            set { lightIntensityParam.SetValue(value); }
        }

        public Vector3 CameraPosition
        {
            get { return cameraPositionParam.GetValueVector3(); }
            set { cameraPositionParam.SetValue(value); }
        }

        public Matrix InvertViewProjection
        {
            get { return invertViewProjectionParam.GetValueMatrix(); }
            set { invertViewProjectionParam.SetValue(value); }
        }

        public Matrix Projection
        {
            get { return projectionParam.GetValueMatrix(); }
            set { projectionParam.SetValue(value); }
        }

        public Matrix View
        {
            get { return viewParam.GetValueMatrix(); }
            set { viewParam.SetValue(value); }
        }

        public Matrix World
        {
            get { return worldParam.GetValueMatrix(); }
            set { worldParam.SetValue(value); }
        }

        #endregion

        #region Methods

         public DeferredPointLightEffect(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, 
#if NETFX_CORE
            LoadEffectResourceStream(resourceName), true
#else
            LoadEffectResource(resourceName)
#endif
           )
        {    
            CacheEffectParameters(null);
        }

        public DeferredPointLightEffect(GraphicsDevice graphicsDevice, byte[] Bytecode): base(graphicsDevice, Bytecode)
        {   
            CacheEffectParameters(null);
        }

        protected DeferredPointLightEffect(DeferredPointLightEffect cloneSource)
            : base(cloneSource)
        {
            CacheEffectParameters(cloneSource);
        }
        
        public override Effect Clone()
        {
            return new DeferredPointLightEffect(this);
        }

        void CacheEffectParameters(DeferredPointLightEffect cloneSource)
        {
            colorMapParam = Parameters["colorMap"];
            normalMapParam = Parameters["normalMap"];
            depthMapParam = Parameters["depthMap"];
            halfPixelParam = Parameters["halfPixel"];

            lightPositionParam = Parameters["lightPosition"];
            colorParam = Parameters["Color"];
            lightRadiusParam = Parameters["lightRadius"];
            lightIntensityParam = Parameters["lightIntensity"];
            cameraPositionParam = Parameters["cameraPosition"];
            invertViewProjectionParam = Parameters["InvertViewProjection"];

            // IEffectMatrices
            projectionParam = Parameters["Projection"];
            viewParam = Parameters["View"];
            worldParam = Parameters["World"];
        }
        
        #endregion
    }
}
