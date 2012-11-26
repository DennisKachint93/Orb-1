Shader "Particles/AdditiveBlend" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	AlphaTest Greater .01
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	// ---- Fragment program cards
	SubShader {
		Pass {
		
			Program "vp" {
// Vertex combos: 2
//   opengl - ALU: 8 to 16
//   d3d9 - ALU: 8 to 16
SubProgram "opengl " {
Keywords { "SOFTPARTICLES_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Vector 5 [_MainTex_ST]
"!!ARBvp1.0
# 8 ALU
PARAM c[6] = { program.local[0],
		state.matrix.mvp,
		program.local[5] };
MOV result.color, vertex.color;
MAD result.texcoord[0].xy, vertex.texcoord[0], c[5], c[5].zwzw;
MAD result.texcoord[2].xy, vertex.attrib[14], c[5], c[5].zwzw;
DP4 result.position.w, vertex.position, c[4];
DP4 result.position.z, vertex.position, c[3];
DP4 result.position.y, vertex.position, c[2];
DP4 result.position.x, vertex.position, c[1];
MOV result.texcoord[2].z, vertex.attrib[14];
END
# 8 instructions, 0 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "SOFTPARTICLES_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_ST]
"vs_2_0
; 8 ALU
dcl_position0 v0
dcl_color0 v1
dcl_texcoord0 v2
dcl_tangent0 v3
mov oD0, v1
mad oT0.xy, v2, c4, c4.zwzw
mad oT2.xy, v3, c4, c4.zwzw
dp4 oPos.w, v0, c3
dp4 oPos.z, v0, c2
dp4 oPos.y, v0, c1
dp4 oPos.x, v0, c0
mov oT2.z, v3
"
}

SubProgram "gles " {
Keywords { "SOFTPARTICLES_OFF" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec3 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;

uniform highp vec4 _MainTex_ST;
attribute vec4 _glesTANGENT;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesColor;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize (_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2.xy = ((tmpvar_1.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_2.z = tmpvar_1.z;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD2 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

varying highp vec3 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
uniform lowp vec4 _TintColor;
uniform sampler2D _MainTex;
void main ()
{
  lowp vec4 tex;
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD2.xy);
  highp vec4 tmpvar_3;
  tmpvar_3 = mix (tmpvar_1, tmpvar_2, xlv_TEXCOORD2.zzzz);
  tex = tmpvar_3;
  gl_FragData[0] = (((2.0 * xlv_COLOR) * _TintColor) * tex);
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "SOFTPARTICLES_OFF" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;

varying highp vec3 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;

uniform highp vec4 _MainTex_ST;
attribute vec4 _glesTANGENT;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesColor;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize (_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec3 tmpvar_2;
  tmpvar_2.xy = ((tmpvar_1.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_2.z = tmpvar_1.z;
  gl_Position = (gl_ModelViewProjectionMatrix * _glesVertex);
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD2 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

varying highp vec3 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
uniform lowp vec4 _TintColor;
uniform sampler2D _MainTex;
void main ()
{
  lowp vec4 tex;
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD2.xy);
  highp vec4 tmpvar_3;
  tmpvar_3 = mix (tmpvar_1, tmpvar_2, xlv_TEXCOORD2.zzzz);
  tex = tmpvar_3;
  gl_FragData[0] = (((2.0 * xlv_COLOR) * _TintColor) * tex);
}



#endif"
}

SubProgram "flash " {
Keywords { "SOFTPARTICLES_OFF" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_mvp]
Vector 4 [_MainTex_ST]
"agal_vs
[bc]
aaaaaaaaahaaapaeacaaaaoeaaaaaaaaaaaaaaaaaaaaaaaa mov v7, a2
adaaaaaaaaaaadacadaaaaoeaaaaaaaaaeaaaaoeabaaaaaa mul r0.xy, a3, c4
abaaaaaaaaaaadaeaaaaaafeacaaaaaaaeaaaaooabaaaaaa add v0.xy, r0.xyyy, c4.zwzw
adaaaaaaaaaaadacafaaaaoeaaaaaaaaaeaaaaoeabaaaaaa mul r0.xy, a5, c4
abaaaaaaacaaadaeaaaaaafeacaaaaaaaeaaaaooabaaaaaa add v2.xy, r0.xyyy, c4.zwzw
bdaaaaaaaaaaaiadaaaaaaoeaaaaaaaaadaaaaoeabaaaaaa dp4 o0.w, a0, c3
bdaaaaaaaaaaaeadaaaaaaoeaaaaaaaaacaaaaoeabaaaaaa dp4 o0.z, a0, c2
bdaaaaaaaaaaacadaaaaaaoeaaaaaaaaabaaaaoeabaaaaaa dp4 o0.y, a0, c1
bdaaaaaaaaaaabadaaaaaaoeaaaaaaaaaaaaaaoeabaaaaaa dp4 o0.x, a0, c0
aaaaaaaaacaaaeaeafaaaaoeaaaaaaaaaaaaaaaaaaaaaaaa mov v2.z, a5
aaaaaaaaaaaaamaeaaaaaaoeabaaaaaaaaaaaaaaaaaaaaaa mov v0.zw, c0
aaaaaaaaacaaaiaeaaaaaaoeabaaaaaaaaaaaaaaaaaaaaaa mov v2.w, c0
"
}

SubProgram "opengl " {
Keywords { "SOFTPARTICLES_ON" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "tangent" ATTR14
Vector 9 [_ProjectionParams]
Vector 10 [_MainTex_ST]
"!!ARBvp1.0
# 16 ALU
PARAM c[11] = { { 0.5 },
		state.matrix.modelview[0],
		state.matrix.mvp,
		program.local[9..10] };
TEMP R0;
TEMP R1;
DP4 R1.w, vertex.position, c[8];
DP4 R0.x, vertex.position, c[5];
MOV R0.w, R1;
DP4 R0.y, vertex.position, c[6];
MUL R1.xyz, R0.xyww, c[0].x;
MUL R1.y, R1, c[9].x;
DP4 R0.z, vertex.position, c[7];
MOV result.position, R0;
DP4 R0.x, vertex.position, c[3];
ADD result.texcoord[1].xy, R1, R1.z;
MOV result.color, vertex.color;
MAD result.texcoord[0].xy, vertex.texcoord[0], c[10], c[10].zwzw;
MAD result.texcoord[2].xy, vertex.attrib[14], c[10], c[10].zwzw;
MOV result.texcoord[1].z, -R0.x;
MOV result.texcoord[1].w, R1;
MOV result.texcoord[2].z, vertex.attrib[14];
END
# 16 instructions, 2 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "SOFTPARTICLES_ON" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_modelview0]
Matrix 4 [glstate_matrix_mvp]
Vector 8 [_ProjectionParams]
Vector 9 [_ScreenParams]
Vector 10 [_MainTex_ST]
"vs_2_0
; 16 ALU
def c11, 0.50000000, 0, 0, 0
dcl_position0 v0
dcl_color0 v1
dcl_texcoord0 v2
dcl_tangent0 v3
dp4 r1.w, v0, c7
dp4 r0.x, v0, c4
mov r0.w, r1
dp4 r0.y, v0, c5
mul r1.xyz, r0.xyww, c11.x
mul r1.y, r1, c8.x
dp4 r0.z, v0, c6
mov oPos, r0
dp4 r0.x, v0, c2
mad oT1.xy, r1.z, c9.zwzw, r1
mov oD0, v1
mad oT0.xy, v2, c10, c10.zwzw
mad oT2.xy, v3, c10, c10.zwzw
mov oT1.z, -r0.x
mov oT1.w, r1
mov oT2.z, v3
"
}

SubProgram "gles " {
Keywords { "SOFTPARTICLES_ON" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
#define gl_ModelViewMatrix glstate_matrix_modelview0
uniform mat4 glstate_matrix_modelview0;

varying highp vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;


uniform highp vec4 _ProjectionParams;
uniform highp vec4 _MainTex_ST;
attribute vec4 _glesTANGENT;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesColor;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize (_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec4 tmpvar_2;
  highp vec3 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (gl_ModelViewProjectionMatrix * _glesVertex);
  highp vec4 o_i0;
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_4 * 0.5);
  o_i0 = tmpvar_5;
  highp vec2 tmpvar_6;
  tmpvar_6.x = tmpvar_5.x;
  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
  o_i0.xy = (tmpvar_6 + tmpvar_5.w);
  o_i0.zw = tmpvar_4.zw;
  tmpvar_2 = o_i0;
  tmpvar_2.z = -((gl_ModelViewMatrix * _glesVertex).z);
  tmpvar_3.xy = ((tmpvar_1.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.z = tmpvar_1.z;
  gl_Position = tmpvar_4;
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

varying highp vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
uniform highp vec4 _ZBufferParams;
uniform lowp vec4 _TintColor;
uniform sampler2D _MainTex;
uniform highp float _InvFade;
uniform sampler2D _CameraDepthTexture;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = xlv_COLOR;
  lowp vec4 tex;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2DProj (_CameraDepthTexture, xlv_TEXCOORD1);
  highp float z;
  z = tmpvar_2.x;
  highp float tmpvar_3;
  tmpvar_3 = (xlv_COLOR.w * clamp ((_InvFade * ((1.0/(((_ZBufferParams.z * z) + _ZBufferParams.w))) - xlv_TEXCOORD1.z)), 0.0, 1.0));
  tmpvar_1.w = tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD2.xy);
  highp vec4 tmpvar_6;
  tmpvar_6 = mix (tmpvar_4, tmpvar_5, xlv_TEXCOORD2.zzzz);
  tex = tmpvar_6;
  gl_FragData[0] = (((2.0 * tmpvar_1) * _TintColor) * tex);
}



#endif"
}

SubProgram "glesdesktop " {
Keywords { "SOFTPARTICLES_ON" }
"!!GLES
#define SHADER_API_GLES 1
#define tex2D texture2D


#ifdef VERTEX
#define gl_ModelViewProjectionMatrix glstate_matrix_mvp
uniform mat4 glstate_matrix_mvp;
#define gl_ModelViewMatrix glstate_matrix_modelview0
uniform mat4 glstate_matrix_modelview0;

varying highp vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;


uniform highp vec4 _ProjectionParams;
uniform highp vec4 _MainTex_ST;
attribute vec4 _glesTANGENT;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesColor;
attribute vec4 _glesVertex;
void main ()
{
  vec4 tmpvar_1;
  tmpvar_1.xyz = normalize (_glesTANGENT.xyz);
  tmpvar_1.w = _glesTANGENT.w;
  highp vec4 tmpvar_2;
  highp vec3 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (gl_ModelViewProjectionMatrix * _glesVertex);
  highp vec4 o_i0;
  highp vec4 tmpvar_5;
  tmpvar_5 = (tmpvar_4 * 0.5);
  o_i0 = tmpvar_5;
  highp vec2 tmpvar_6;
  tmpvar_6.x = tmpvar_5.x;
  tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
  o_i0.xy = (tmpvar_6 + tmpvar_5.w);
  o_i0.zw = tmpvar_4.zw;
  tmpvar_2 = o_i0;
  tmpvar_2.z = -((gl_ModelViewMatrix * _glesVertex).z);
  tmpvar_3.xy = ((tmpvar_1.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_3.z = tmpvar_1.z;
  gl_Position = tmpvar_4;
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

varying highp vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
uniform highp vec4 _ZBufferParams;
uniform lowp vec4 _TintColor;
uniform sampler2D _MainTex;
uniform highp float _InvFade;
uniform sampler2D _CameraDepthTexture;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = xlv_COLOR;
  lowp vec4 tex;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2DProj (_CameraDepthTexture, xlv_TEXCOORD1);
  highp float z;
  z = tmpvar_2.x;
  highp float tmpvar_3;
  tmpvar_3 = (xlv_COLOR.w * clamp ((_InvFade * ((1.0/(((_ZBufferParams.z * z) + _ZBufferParams.w))) - xlv_TEXCOORD1.z)), 0.0, 1.0));
  tmpvar_1.w = tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD2.xy);
  highp vec4 tmpvar_6;
  tmpvar_6 = mix (tmpvar_4, tmpvar_5, xlv_TEXCOORD2.zzzz);
  tex = tmpvar_6;
  gl_FragData[0] = (((2.0 * tmpvar_1) * _TintColor) * tex);
}



#endif"
}

SubProgram "flash " {
Keywords { "SOFTPARTICLES_ON" }
Bind "vertex" Vertex
Bind "color" Color
Bind "texcoord" TexCoord0
Bind "tangent" TexCoord2
Matrix 0 [glstate_matrix_modelview0]
Matrix 4 [glstate_matrix_mvp]
Vector 8 [_ProjectionParams]
Vector 9 [unity_NPOTScale]
Vector 10 [_MainTex_ST]
"agal_vs
c11 0.5 0.0 0.0 0.0
[bc]
bdaaaaaaabaaaiacaaaaaaoeaaaaaaaaahaaaaoeabaaaaaa dp4 r1.w, a0, c7
bdaaaaaaaaaaabacaaaaaaoeaaaaaaaaaeaaaaoeabaaaaaa dp4 r0.x, a0, c4
aaaaaaaaaaaaaiacabaaaappacaaaaaaaaaaaaaaaaaaaaaa mov r0.w, r1.w
bdaaaaaaaaaaacacaaaaaaoeaaaaaaaaafaaaaoeabaaaaaa dp4 r0.y, a0, c5
adaaaaaaabaaahacaaaaaapeacaaaaaaalaaaaaaabaaaaaa mul r1.xyz, r0.xyww, c11.x
adaaaaaaabaaacacabaaaaffacaaaaaaaiaaaaaaabaaaaaa mul r1.y, r1.y, c8.x
abaaaaaaabaaadacabaaaafeacaaaaaaabaaaakkacaaaaaa add r1.xy, r1.xyyy, r1.z
bdaaaaaaaaaaaeacaaaaaaoeaaaaaaaaagaaaaoeabaaaaaa dp4 r0.z, a0, c6
aaaaaaaaaaaaapadaaaaaaoeacaaaaaaaaaaaaaaaaaaaaaa mov o0, r0
bdaaaaaaaaaaabacaaaaaaoeaaaaaaaaacaaaaoeabaaaaaa dp4 r0.x, a0, c2
adaaaaaaabaaadaeabaaaafeacaaaaaaajaaaaoeabaaaaaa mul v1.xy, r1.xyyy, c9
aaaaaaaaahaaapaeacaaaaoeaaaaaaaaaaaaaaaaaaaaaaaa mov v7, a2
adaaaaaaacaaadacadaaaaoeaaaaaaaaakaaaaoeabaaaaaa mul r2.xy, a3, c10
abaaaaaaaaaaadaeacaaaafeacaaaaaaakaaaaooabaaaaaa add v0.xy, r2.xyyy, c10.zwzw
adaaaaaaacaaadacafaaaaoeaaaaaaaaakaaaaoeabaaaaaa mul r2.xy, a5, c10
abaaaaaaacaaadaeacaaaafeacaaaaaaakaaaaooabaaaaaa add v2.xy, r2.xyyy, c10.zwzw
bfaaaaaaabaaaeaeaaaaaaaaacaaaaaaaaaaaaaaaaaaaaaa neg v1.z, r0.x
aaaaaaaaabaaaiaeabaaaappacaaaaaaaaaaaaaaaaaaaaaa mov v1.w, r1.w
aaaaaaaaacaaaeaeafaaaaoeaaaaaaaaaaaaaaaaaaaaaaaa mov v2.z, a5
aaaaaaaaaaaaamaeaaaaaaoeabaaaaaaaaaaaaaaaaaaaaaa mov v0.zw, c0
aaaaaaaaacaaaiaeaaaaaaoeabaaaaaaaaaaaaaaaaaaaaaa mov v2.w, c0
"
}

}
Program "fp" {
// Fragment combos: 2
//   opengl - ALU: 7 to 14, TEX: 2 to 3
//   d3d9 - ALU: 6 to 12, TEX: 2 to 3
SubProgram "opengl " {
Keywords { "SOFTPARTICLES_OFF" }
Vector 0 [_TintColor]
SetTexture 0 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 7 ALU, 2 TEX
PARAM c[2] = { program.local[0],
		{ 2 } };
TEMP R0;
TEMP R1;
TEX R0, fragment.texcoord[0], texture[0], 2D;
TEX R1, fragment.texcoord[2], texture[0], 2D;
ADD R1, R1, -R0;
MAD R1, fragment.texcoord[2].z, R1, R0;
MUL R0, fragment.color.primary, c[0];
MUL R0, R0, R1;
MUL result.color, R0, c[1].x;
END
# 7 instructions, 2 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "SOFTPARTICLES_OFF" }
Vector 0 [_TintColor]
SetTexture 0 [_MainTex] 2D
"ps_2_0
; 6 ALU, 2 TEX
dcl_2d s0
def c1, 2.00000000, 0, 0, 0
dcl v0
dcl t0.xy
dcl t2.xyz
texld r1, t0, s0
texld r0, t2, s0
add_pp r0, r0, -r1
mad_pp r1, t2.z, r0, r1
mul r0, v0, c0
mul r0, r0, r1
mul r0, r0, c1.x
mov_pp oC0, r0
"
}

SubProgram "gles " {
Keywords { "SOFTPARTICLES_OFF" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "SOFTPARTICLES_OFF" }
"!!GLES"
}

SubProgram "flash " {
Keywords { "SOFTPARTICLES_OFF" }
Vector 0 [_TintColor]
SetTexture 0 [_MainTex] 2D
"agal_ps
c1 2.0 0.0 0.0 0.0
[bc]
ciaaaaaaabaaapacaaaaaaoeaeaaaaaaaaaaaaaaafaababb tex r1, v0, s0 <2d wrap linear point>
ciaaaaaaaaaaapacacaaaaoeaeaaaaaaaaaaaaaaafaababb tex r0, v2, s0 <2d wrap linear point>
acaaaaaaaaaaapacaaaaaaoeacaaaaaaabaaaaoeacaaaaaa sub r0, r0, r1
adaaaaaaacaaapacacaaaakkaeaaaaaaaaaaaaoeacaaaaaa mul r2, v2.z, r0
abaaaaaaabaaapacacaaaaoeacaaaaaaabaaaaoeacaaaaaa add r1, r2, r1
adaaaaaaaaaaapacahaaaaoeaeaaaaaaaaaaaaoeabaaaaaa mul r0, v7, c0
adaaaaaaaaaaapacaaaaaaoeacaaaaaaabaaaaoeacaaaaaa mul r0, r0, r1
adaaaaaaaaaaapacaaaaaaoeacaaaaaaabaaaaaaabaaaaaa mul r0, r0, c1.x
aaaaaaaaaaaaapadaaaaaaoeacaaaaaaaaaaaaaaaaaaaaaa mov o0, r0
"
}

SubProgram "opengl " {
Keywords { "SOFTPARTICLES_ON" }
Vector 0 [_ZBufferParams]
Vector 1 [_TintColor]
Float 2 [_InvFade]
SetTexture 0 [_CameraDepthTexture] 2D
SetTexture 1 [_MainTex] 2D
"!!ARBfp1.0
OPTION ARB_precision_hint_fastest;
# 14 ALU, 3 TEX
PARAM c[4] = { program.local[0..2],
		{ 2 } };
TEMP R0;
TEMP R1;
TEMP R2;
TXP R2.x, fragment.texcoord[1], texture[0], 2D;
TEX R0, fragment.texcoord[0], texture[1], 2D;
TEX R1, fragment.texcoord[2], texture[1], 2D;
ADD R1, R1, -R0;
MAD R1, fragment.texcoord[2].z, R1, R0;
MAD R2.x, R2, c[0].z, c[0].w;
RCP R2.x, R2.x;
ADD R2.x, R2, -fragment.texcoord[1].z;
MUL_SAT R0.w, R2.x, c[2].x;
MOV R0.xyz, fragment.color.primary;
MUL R0.w, fragment.color.primary, R0;
MUL R0, R0, c[1];
MUL R0, R0, R1;
MUL result.color, R0, c[3].x;
END
# 14 instructions, 3 R-regs
"
}

SubProgram "d3d9 " {
Keywords { "SOFTPARTICLES_ON" }
Vector 0 [_ZBufferParams]
Vector 1 [_TintColor]
Float 2 [_InvFade]
SetTexture 0 [_CameraDepthTexture] 2D
SetTexture 1 [_MainTex] 2D
"ps_2_0
; 12 ALU, 3 TEX
dcl_2d s0
dcl_2d s1
def c3, 2.00000000, 0, 0, 0
dcl v0
dcl t0.xy
dcl t1
dcl t2.xyz
texldp r0, t1, s0
texld r2, t2, s1
texld r1, t0, s1
mad r0.x, r0, c0.z, c0.w
rcp r0.x, r0.x
add r0.x, r0, -t1.z
add_pp r2, r2, -r1
mad_pp r1, t2.z, r2, r1
mul_sat r0.x, r0, c2
mov_pp r2.xyz, v0
mul_pp r2.w, v0, r0.x
mul r0, r2, c1
mul r0, r0, r1
mul r0, r0, c3.x
mov_pp oC0, r0
"
}

SubProgram "gles " {
Keywords { "SOFTPARTICLES_ON" }
"!!GLES"
}

SubProgram "glesdesktop " {
Keywords { "SOFTPARTICLES_ON" }
"!!GLES"
}

SubProgram "flash " {
Keywords { "SOFTPARTICLES_ON" }
Vector 0 [_ZBufferParams]
Vector 1 [_TintColor]
Float 2 [_InvFade]
SetTexture 0 [_CameraDepthTexture] 2D
SetTexture 1 [_MainTex] 2D
"agal_ps
c3 1.0 0.003922 0.000015 0.0
c4 2.0 0.0 0.0 0.0
[bc]
ciaaaaaaacaaapacacaaaaoeaeaaaaaaabaaaaaaafaababb tex r2, v2, s1 <2d wrap linear point>
aeaaaaaaaaaaapacabaaaaoeaeaaaaaaabaaaappaeaaaaaa div r0, v1, v1.w
ciaaaaaaaaaaapacaaaaaafeacaaaaaaaaaaaaaaafaababb tex r0, r0.xyyy, s0 <2d wrap linear point>
ciaaaaaaabaaapacaaaaaaoeaeaaaaaaabaaaaaaafaababb tex r1, v0, s1 <2d wrap linear point>
bdaaaaaaaaaaabacaaaaaaoeacaaaaaaadaaaaoeabaaaaaa dp4 r0.x, r0, c3
adaaaaaaaaaaabacaaaaaaaaacaaaaaaaaaaaakkabaaaaaa mul r0.x, r0.x, c0.z
abaaaaaaaaaaabacaaaaaaaaacaaaaaaaaaaaappabaaaaaa add r0.x, r0.x, c0.w
afaaaaaaaaaaabacaaaaaaaaacaaaaaaaaaaaaaaaaaaaaaa rcp r0.x, r0.x
acaaaaaaaaaaabacaaaaaaaaacaaaaaaabaaaakkaeaaaaaa sub r0.x, r0.x, v1.z
acaaaaaaacaaapacacaaaaoeacaaaaaaabaaaaoeacaaaaaa sub r2, r2, r1
adaaaaaaadaaapacacaaaakkaeaaaaaaacaaaaoeacaaaaaa mul r3, v2.z, r2
abaaaaaaabaaapacadaaaaoeacaaaaaaabaaaaoeacaaaaaa add r1, r3, r1
adaaaaaaaaaaabacaaaaaaaaacaaaaaaacaaaaoeabaaaaaa mul r0.x, r0.x, c2
bgaaaaaaaaaaabacaaaaaaaaacaaaaaaaaaaaaaaaaaaaaaa sat r0.x, r0.x
aaaaaaaaacaaahacahaaaaoeaeaaaaaaaaaaaaaaaaaaaaaa mov r2.xyz, v7
adaaaaaaacaaaiacahaaaaoeaeaaaaaaaaaaaaaaacaaaaaa mul r2.w, v7, r0.x
adaaaaaaaaaaapacacaaaaoeacaaaaaaabaaaaoeabaaaaaa mul r0, r2, c1
adaaaaaaaaaaapacaaaaaaoeacaaaaaaabaaaaoeacaaaaaa mul r0, r0, r1
adaaaaaaaaaaapacaaaaaaoeacaaaaaaaeaaaaaaabaaaaaa mul r0, r0, c4.x
aaaaaaaaaaaaapadaaaaaaoeacaaaaaaaaaaaaaaaaaaaaaa mov o0, r0
"
}

}

#LINE 88
 
		}
	} 	
	
	// ---- Dual texture cards
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				constantColor [_TintColor]
				combine constant * primary
			}
			SetTexture [_MainTex] {
				combine texture * previous DOUBLE
			}
		}
	}
	
	// ---- Single texture cards (does not do color tint)
	SubShader {
		Pass {
			SetTexture [_MainTex] {
				combine texture * primary
			}
		}
	}
}
}
