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
    public class DeferredSpotLightEffect : Effect, IEffectMatrices
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

        EffectParameter lightDirectionParam;
        EffectParameter innerAngleCosParam;
        EffectParameter outerAngleCosParam;

        // IEffectMatrices
        EffectParameter projectionParam;
        EffectParameter viewParam;
        EffectParameter worldParam;

        #endregion

        #region Fields


        static readonly string ResourceName = "nkast.Aether.Shaders.Resources.DeferredSpotLight";

        internal static byte[] LoadEffectResource(GraphicsDevice graphicsDevice, string name)
        {
            name = GetResourceName(graphicsDevice, name);
            using (Stream stream = typeof(DeferredPointLightEffect).Assembly.GetManifestResourceStream(name))
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

        public Vector3 LightDirection
        {
            get { return lightDirectionParam.GetValueVector3(); }
            set { lightDirectionParam.SetValue(value); }
        }

        public float InnerAngleCos
        {
            get { return innerAngleCosParam.GetValueSingle(); }
            set { innerAngleCosParam.SetValue(value); }
        }

        public float OuterAngleCos
        {
            get { return outerAngleCosParam.GetValueSingle(); }
            set { outerAngleCosParam.SetValue(value); }
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

         public DeferredSpotLightEffect(GraphicsDevice graphicsDevice)
            : base(graphicsDevice, LoadEffectResource(graphicsDevice, ResourceName))
        {    
            CacheEffectParameters(null);
        }

        public DeferredSpotLightEffect(GraphicsDevice graphicsDevice, byte[] Bytecode): base(graphicsDevice, Bytecode)
        {   
            CacheEffectParameters(null);
        }

        protected DeferredSpotLightEffect(DeferredSpotLightEffect cloneSource)
            : base(cloneSource)
        {
            CacheEffectParameters(cloneSource);
        }
        
        public override Effect Clone()
        {
            return new DeferredSpotLightEffect(this);
        }

        void CacheEffectParameters(DeferredSpotLightEffect cloneSource)
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

            lightDirectionParam = Parameters["lightDirection"];
            innerAngleCosParam = Parameters["innerAngleCos"];
            outerAngleCosParam = Parameters["outerAngleCos"];

            // IEffectMatrices
            projectionParam = Parameters["Projection"];
            viewParam = Parameters["View"];
            worldParam = Parameters["World"];
        }
        
        #endregion
    }
}
