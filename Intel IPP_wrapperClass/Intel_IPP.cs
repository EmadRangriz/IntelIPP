/*
    Author: Emad Rangriz Rostami  (rangriz.emad@gmail.com||rangriz.emad@aut.ac.ir)
    this a wrapper on some Intel IPPes Signal&Image processing function.
    all IPPes function are assembled in a dll named "intel_customized",
    also, for some functions like "Intel Voice Activity Detection," 
    Intel legacy "ippsc90lgc" has been used.
 */

using System;
using System.Runtime.InteropServices;

namespace Intel_IPP_class
{
    public struct ComplexF : IComparable, ICloneable
    {

        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        /// <summary>
        /// The real component of the complex number
        /// </summary>
        public float Re;

        /// <summary>
        /// The imaginary component of the complex number
        /// </summary>
        public float Im;

        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        /// <summary>
        /// Create a complex number from a real and an imaginary component
        /// </summary>
        /// <param name="real"></param>
        /// <param name="imaginary"></param>
        public ComplexF(float real, float imaginary)
        {
            this.Re = (float)real;
            this.Im = (float)imaginary;
        }

        /// <summary>
        /// Create a complex number based on an existing complex number
        /// </summary>
        /// <param name="c"></param>
        public ComplexF(ComplexF c)
        {
            this.Re = c.Re;
            this.Im = c.Im;
        }

        /// <summary>
        /// Create a complex number from a real and an imaginary component
        /// </summary>
        /// <param name="real"></param>
        /// <param name="imaginary"></param>
        /// <returns></returns>
        static public ComplexF FromRealImaginary(float real, float imaginary)
        {
            ComplexF c;
            c.Re = (float)real;
            c.Im = (float)imaginary;
            return c;
        }

        /// <summary>
        /// Create a complex number from a modulus (length) and an argument (radian)
        /// </summary>
        /// <param name="modulus"></param>
        /// <param name="argument"></param>
        /// <returns></returns>
        static public ComplexF FromModulusArgument(float modulus, float argument)
        {
            ComplexF c;
            c.Re = (float)(modulus * System.Math.Cos(argument));
            c.Im = (float)(modulus * System.Math.Sin(argument));
            return c;
        }

        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        object ICloneable.Clone()
        {
            return new ComplexF(this);
        }
        /// <summary>
        /// Clone the complex number
        /// </summary>
        /// <returns></returns>
        public ComplexF Clone()
        {
            return new ComplexF(this);
        }

        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        /// <summary>
        /// The modulus (length) of the complex number
        /// </summary>
        /// <returns></returns>
        public float GetModulus()
        {
            float x = this.Re;
            float y = this.Im;
            return (float)Math.Sqrt(x * x + y * y);
        }

        /// <summary>
        /// The squared modulus (length^2) of the complex number
        /// </summary>
        /// <returns></returns>
        public float GetModulusSquared()
        {
            float x = this.Re;
            float y = this.Im;
            return (float)x * x + y * y;
        }

        /// <summary>
        /// The argument (radians) of the complex number
        /// </summary>
        /// <returns></returns>
        public float GetArgument()
        {
            return (float)Math.Atan2(this.Im, this.Re);
        }

        //-----------------------------------------------------------------------------------

        /// <summary>
        /// Get the conjugate of the complex number
        /// </summary>
        /// <returns></returns>
        public ComplexF GetConjugate()
        {
            return FromRealImaginary(this.Re, -this.Im);
        }

        //-----------------------------------------------------------------------------------

        /// <summary>
        /// Scale the complex number to 1.
        /// </summary>
        public void Normalize()
        {
            double modulus = this.GetModulus();
            if (modulus == 0)
            {
                throw new DivideByZeroException("Can not normalize a complex number that is zero.");
            }
            this.Re = (float)(this.Re / modulus);
            this.Im = (float)(this.Im / modulus);
        }
        

        /// <summary>
        /// Convert from a single precision real number to a complex number
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static explicit operator ComplexF(float f)
        {
            ComplexF c;
            c.Re = (float)f;
            c.Im = (float)0;
            return c;
        }

        /// <summary>
        /// Convert from a single precision complex to a real number
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static explicit operator float(ComplexF c)
        {
            return (float)c.Re;
        }
        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        /// <summary>
        /// Are these two complex numbers equivalent?
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(ComplexF a, ComplexF b)
        {
            return (a.Re == b.Re) && (a.Im == b.Im);
        }

        /// <summary>
        /// Are these two complex numbers different?
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(ComplexF a, ComplexF b)
        {
            return (a.Re != b.Re) || (a.Im != b.Im);
        }

        /// <summary>
        /// Get the hash code of the complex number
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (this.Re.GetHashCode() ^ this.Im.GetHashCode());
        }

        /// <summary>
        /// Is this complex number equivalent to another object?
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override bool Equals(object o)
        {
            if (o is ComplexF)
            {
                ComplexF c = (ComplexF)o;
                return (this == c);
            }
            return false;
        }

        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        /// <summary>
        /// Compare to other complex numbers or real numbers
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int CompareTo(object o)
        {
            if (o == null)
            {
                return 1;  // null sorts before current
            }
            if (o is ComplexF)
            {
                return this.GetModulus().CompareTo(((ComplexF)o).GetModulus());
            }
            if (o is float)
            {
                return this.GetModulus().CompareTo((float)o);
            }
            //    if (o is Complex)
            //    {
            //      return this.GetModulus().CompareTo(((Complex)o).GetModulus());
            //   }
            if (o is double)
            {
                return this.GetModulus().CompareTo((double)o);
            }
            throw new ArgumentException();
        }

        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        /// <summary>
        /// This operator doesn't do much. :-)
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static ComplexF operator +(ComplexF a)
        {
            return a;
        }

        /// <summary>
        /// Negate the complex number
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static ComplexF operator -(ComplexF a)
        {
            a.Re = -a.Re;
            a.Im = -a.Im;
            return a;
        }

        /// <summary>
        /// Add a complex number to a real
        /// </summary>
        /// <param name="a"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static ComplexF operator +(ComplexF a, float f)
        {
            a.Re = (float)(a.Re + f);
            return a;
        }

        /// <summary>
        /// Add a real to a complex number
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static ComplexF operator +(float f, ComplexF a)
        {
            a.Re = (float)(a.Re + f);
            return a;
        }

        /// <summary>
        /// Add to complex numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ComplexF operator +(ComplexF a, ComplexF b)
        {
            a.Re = a.Re + b.Re;
            a.Im = a.Im + b.Im;
            return a;
        }

        /// <summary>
        /// Subtract a real from a complex number
        /// </summary>
        /// <param name="a"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static ComplexF operator -(ComplexF a, float f)
        {
            a.Re = (float)(a.Re - f);
            return a;
        }

        /// <summary>
        /// Subtract a complex number from a real
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static ComplexF operator -(float f, ComplexF a)
        {
            a.Re = (float)(f - a.Re);
            a.Im = (float)(0 - a.Im);
            return a;
        }

        /// <summary>
        /// Subtract two complex numbers
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ComplexF operator -(ComplexF a, ComplexF b)
        {
            a.Re = a.Re - b.Re;
            a.Im = a.Im - b.Im;
            return a;
        }

        /// <summary>
        /// Multiply a complex number by a real
        /// </summary>
        /// <param name="a"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static ComplexF operator *(ComplexF a, float f)
        {
            a.Re = (float)(a.Re * f);
            a.Im = (float)(a.Im * f);
            return a;
        }

        /// <summary>
        /// Multiply a real by a complex number
        /// </summary>
        /// <param name="f"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static ComplexF operator *(float f, ComplexF a)
        {
            a.Re = (float)(a.Re * f);
            a.Im = (float)(a.Im * f);
            return a;
        }

        /// <summary>
        /// Multiply two complex numbers together
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ComplexF operator *(ComplexF a, ComplexF b)
        {
            // (x + yi)(u + vi) = (xu – yv) + (xv + yu)i. 
            double x = a.Re, y = a.Im;
            double u = b.Re, v = b.Im;
            a.Re = (float)(x * u - y * v);
            a.Im = (float)(x * v + y * u);
            return a;
        }

        /// <summary>
        /// Divide a complex number by a real number
        /// </summary>
        /// <param name="a"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static ComplexF operator /(ComplexF a, float f)
        {
            if (f == 0)
            {
                throw new DivideByZeroException();
            }
            a.Re = (float)(a.Re / f);
            a.Im = (float)(a.Im / f);
            return a;
        }

        /// <summary>
        /// Divide a complex number by a complex number
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ComplexF operator /(ComplexF a, ComplexF b)
        {
            double x = a.Re, y = a.Im;
            double u = b.Re, v = b.Im;
            double denom = u * u + v * v;

            if (denom == 0)
            {
                throw new DivideByZeroException();
            }
            a.Re = (float)((x * u + y * v) / denom);
            a.Im = (float)((y * u - x * v) / denom);
            return a;
        }

        /// <summary>
        /// Parse a complex representation in this fashion: "( %f, %f )"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static public ComplexF Parse(string s)
        {
            throw new NotImplementedException("ComplexF ComplexF.Parse( string s ) is not implemented.");
        }

        /// <summary>
        /// Get the string representation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("( {0}, {1}i )", this.Re, this.Im);
        }

        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        /// <summary>
        /// Determine whether two complex numbers are almost (i.e. within the tolerance) equivalent.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        static public bool IsEqual(ComplexF a, ComplexF b, float tolerance)
        {
            return
                (Math.Abs(a.Re - b.Re) < tolerance) &&
                (Math.Abs(a.Im - b.Im) < tolerance);

        }

        //----------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------

        /// <summary>
        /// Represents zero
        /// </summary>
        static public ComplexF Zero
        {
            get { return new ComplexF(0, 0); }
        }

        /// <summary>
        /// Represents the result of sqrt( -1 )
        /// </summary>
        static public ComplexF I
        {
            get { return new ComplexF(0, 1); }
        }

        /// <summary>
        /// Represents the largest possible value of ComplexF.
        /// </summary>
        static public ComplexF MaxValue
        {
            get { return new ComplexF(float.MaxValue, float.MaxValue); }
        }

        /// <summary>
        /// Represents the smallest possible value of ComplexF.
        /// </summary>
        static public ComplexF MinValue
        {
            get { return new ComplexF(float.MinValue, float.MinValue); }
        }


        //----------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------
    }
    public struct ComplexS : IComparable, ICloneable
    {
        /// <summary>
        /// The real component of the complex number
        /// </summary>
        public short Re;

        /// <summary>
        /// The imaginary component of the complex number
        /// </summary>
        public short Im;

        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        /// <summary>
        /// Create a complex number from a real and an imaginary component
        /// </summary>
        /// <param name="real"></param>
        /// <param name="imaginary"></param>
        public ComplexS(short real, short imaginary)
        {
            this.Re = (short)real;
            this.Im = (short)imaginary;
        }

        /// <summary>
        /// Create a complex number based on an existing complex number
        /// </summary>
        /// <param name="c"></param>
        public ComplexS(ComplexS c)
        {
            this.Re = c.Re;
            this.Im = c.Im;
        }

        /// <summary>
        /// Create a complex number from a real and an imaginary component
        /// </summary>
        /// <param name="real"></param>
        /// <param name="imaginary"></param>
        /// <returns></returns>
        static public ComplexS FromRealImaginary(short real, short imaginary)
        {
            ComplexS c;
            c.Re = (short)real;
            c.Im = (short)imaginary;
            return c;
        }

        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        object ICloneable.Clone()
        {
            return new ComplexS(this);
        }
        /// <summary>
        /// Clone the complex number
        /// </summary>
        /// <returns></returns>
        public ComplexS Clone()
        {
            return new ComplexS(this);
        }

        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        /// <summary>
        /// The modulus (length) of the complex number
        /// </summary>
        /// <returns></returns>
        public float GetModulus()
        {
            short x = this.Re;
            short y = this.Im;
            return (float)Math.Sqrt(x * x + y * y);
        }


        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------

        /// <summary>
        /// Compare to other complex numbers or real numbers
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public int CompareTo(object o)
        {
            if (o == null)
            {
                return 1;  // null sorts before current
            }
            if (o is ComplexS)
            {
                return this.GetModulus().CompareTo(((ComplexS)o).GetModulus());
            }
            if (o is float)
            {
                return this.GetModulus().CompareTo((float)o);
            }
            //    if (o is Complex)
            //    {
            //      return this.GetModulus().CompareTo(((Complex)o).GetModulus());
            //   }
            if (o is double)
            {
                return this.GetModulus().CompareTo((double)o);
            }
            throw new ArgumentException();
        }
    }
    public class Intel_IPPes
    {
        private const string libname = "intel_customized.dll";
        private const string libname_legacy = "ippsc90lgc.dll";
        public enum IppPCMFrequency
        {
            IPP_PCM_FREQ_8000 = 8000,                /* 8  kHz  */
            IPP_PCM_FREQ_16000 = 16000,               /* 16 kHz */
            IPP_PCM_FREQ_22050 = 22050,               /* 22.05 kHz */
            IPP_PCM_FREQ_32000 = 32000,               /* 32 kHz */
            IPP_PCM_FREQ_11025 = 11025
        }
        public enum IppDataType : ushort
        {
            ipp1u = 0,
            ipp8u = 1,
            ipp8s = 2,
            ipp16u = 3,
            ipp16s = 4,
            ipp16sc = 5,
            ipp32u = 6,
            ipp32s = 7,
            ipp32sc = 8,
            ipp32f = 9,
            ipp32fc = 10,
            ipp64u = 11,
            ipp64s = 12,
            ipp64sc = 13,
            ipp64f = 14,
            ipp64fc = 15,
        };
        public enum IppsNrMode
        {
            ippsNrNoUpdate = -1,
            ippsNrUpdate,
            ippsNrUpdateAll
        }
        public enum IppsNRLevel
        {
            ippsNrNone,
            ippsNrLow,
            ippsNrMedium,
            ippsNrNormal,
            ippsNrHigh,
            ippsNrAuto
        }
        public enum IppsNrSmoothMode
        {
            ippsNrSmoothDynamic = -1,
            ippsNrSmoothStatic,
            ippsNrSmoothOff
        }
        public enum IppStatus
        {
            ippStsNotSupportedModeErr = -9999,
            ippStsCpuNotSupportedErr = -9998,
            ippStsDitherTypeErr = -224,
            ippStsH264BufferFullErr = -223,
            ippStsWrongAffinitySettingErr = -222,
            ippStsLoadDynErr = -221,
            ippStsPointAtInfinity = -220,
            ippStsI18nUnsupportedErr = -219,
            ippStsI18nMsgCatalogOpenErr = -218,
            ippStsI18nMsgCatalogCloseErr = -217,
            ippStsUnknownStatusCodeErr = -216,
            ippStsOFBSizeErr = -215,
            ippStsLzoBrokenStreamErr = -214,
            ippStsRoundModeNotSupportedErr = -213,
            ippStsDecimateFractionErr = -212,
            ippStsWeightErr = -211,
            ippStsQualityIndexErr = -210,
            ippStsIIRPassbandRippleErr = -209,
            ippStsFilterFrequencyErr = -208,
            ippStsFIRGenOrderErr = -207,
            ippStsIIRGenOrderErr = -206,
            ippStsConvergeErr = -205,
            ippStsSizeMatchMatrixErr = -204,
            ippStsCountMatrixErr = -203,
            ippStsRoiShiftMatrixErr = -202,
            ippStsResizeNoOperationErr = -201,
            ippStsSrcDataErr = -200,
            ippStsMaxLenHuffCodeErr = -199,
            ippStsCodeLenTableErr = -198,
            ippStsFreqTableErr = -197,
            ippStsIncompleteContextErr = -196,
            ippStsSingularErr = -195,
            ippStsSparseErr = -194,
            ippStsBitOffsetErr = -193,
            ippStsQPErr = -192,
            ippStsVLCErr = -191,
            ippStsRegExpOptionsErr = -190,
            ippStsRegExpErr = -189,
            ippStsRegExpMatchLimitErr = -188,
            ippStsRegExpQuantifierErr = -187,
            ippStsRegExpGroupingErr = -186,
            ippStsRegExpBackRefErr = -185,
            ippStsRegExpChClassErr = -184,
            ippStsRegExpMetaChErr = -183,
            ippStsStrideMatrixErr = -182,
            ippStsCTRSizeErr = -181,
            ippStsJPEG2KCodeBlockIsNotAttached = -180,
            ippStsNotPosDefErr = -179,
            ippStsEphemeralKeyErr = -178,
            ippStsMessageErr = -177,
            ippStsShareKeyErr = -176,
            ippStsIvalidPublicKey = -175,
            ippStsIvalidPrivateKey = -174,
            ippStsOutOfECErr = -173,
            ippStsECCInvalidFlagErr = -172,
            ippStsMP3FrameHeaderErr = -171,
            ippStsMP3SideInfoErr = -170,
            ippStsBlockStepErr = -169,
            ippStsMBStepErr = -168,
            ippStsAacPrgNumErr = -167,
            ippStsAacSectCbErr = -166,
            ippStsAacSfValErr = -164,
            ippStsAacCoefValErr = -163,
            ippStsAacMaxSfbErr = -162,
            ippStsAacPredSfbErr = -161,
            ippStsAacPlsDataErr = -160,
            ippStsAacGainCtrErr = -159,
            ippStsAacSectErr = -158,
            ippStsAacTnsNumFiltErr = -157,
            ippStsAacTnsLenErr = -156,
            ippStsAacTnsOrderErr = -155,
            ippStsAacTnsCoefResErr = -154,
            ippStsAacTnsCoefErr = -153,
            ippStsAacTnsDirectErr = -152,
            ippStsAacTnsProfileErr = -151,
            ippStsAacErr = -150,
            ippStsAacBitOffsetErr = -149,
            ippStsAacAdtsSyncWordErr = -148,
            ippStsAacSmplRateIdxErr = -147,
            ippStsAacWinLenErr = -146,
            ippStsAacWinGrpErr = -145,
            ippStsAacWinSeqErr = -144,
            ippStsAacComWinErr = -143,
            ippStsAacStereoMaskErr = -142,
            ippStsAacChanErr = -141,
            ippStsAacMonoStereoErr = -140,
            ippStsAacStereoLayerErr = -139,
            ippStsAacMonoLayerErr = -138,
            ippStsAacScalableErr = -137,
            ippStsAacObjTypeErr = -136,
            ippStsAacWinShapeErr = -135,
            ippStsAacPcmModeErr = -134,
            ippStsVLCUsrTblHeaderErr = -133,
            ippStsVLCUsrTblUnsupportedFmtErr = -132,
            ippStsVLCUsrTblEscAlgTypeErr = -131,
            ippStsVLCUsrTblEscCodeLengthErr = -130,
            ippStsVLCUsrTblCodeLengthErr = -129,
            ippStsVLCInternalTblErr = -128,
            ippStsVLCInputDataErr = -127,
            ippStsVLCAACEscCodeLengthErr = -126,
            ippStsNoiseRangeErr = -125,
            ippStsUnderRunErr = -124,
            ippStsPaddingErr = -123,
            ippStsCFBSizeErr = -122,
            ippStsPaddingSchemeErr = -121,
            ippStsInvalidCryptoKeyErr = -120,
            ippStsLengthErr = -119,
            ippStsBadModulusErr = -118,
            ippStsLPCCalcErr = -117,
            ippStsRCCalcErr = -116,
            ippStsIncorrectLSPErr = -115,
            ippStsNoRootFoundErr = -114,
            ippStsJPEG2KBadPassNumber = -113,
            ippStsJPEG2KDamagedCodeBlock = -112,
            ippStsH263CBPYCodeErr = -111,
            ippStsH263MCBPCInterCodeErr = -110,
            ippStsH263MCBPCIntraCodeErr = -109,
            ippStsNotEvenStepErr = -108,
            ippStsHistoNofLevelsErr = -107,
            ippStsLUTNofLevelsErr = -106,
            ippStsMP4BitOffsetErr = -105,
            ippStsMP4QPErr = -104,
            ippStsMP4BlockIdxErr = -103,
            ippStsMP4BlockTypeErr = -102,
            ippStsMP4MVCodeErr = -101,
            ippStsMP4VLCCodeErr = -100,
            ippStsMP4DCCodeErr = -99,
            ippStsMP4FcodeErr = -98,
            ippStsMP4AlignErr = -97,
            ippStsMP4TempDiffErr = -96,
            ippStsMP4BlockSizeErr = -95,
            ippStsMP4ZeroBABErr = -94,
            ippStsMP4PredDirErr = -93,
            ippStsMP4BitsPerPixelErr = -92,
            ippStsMP4VideoCompModeErr = -91,
            ippStsMP4LinearModeErr = -90,
            ippStsH263PredModeErr = -83,
            ippStsH263BlockStepErr = -82,
            ippStsH263MBStepErr = -81,
            ippStsH263FrameWidthErr = -80,
            ippStsH263FrameHeightErr = -79,
            ippStsH263ExpandPelsErr = -78,
            ippStsH263PlaneStepErr = -77,
            ippStsH263QuantErr = -76,
            ippStsH263MVCodeErr = -75,
            ippStsH263VLCCodeErr = -74,
            ippStsH263DCCodeErr = -73,
            ippStsH263ZigzagLenErr = -72,
            ippStsFBankFreqErr = -71,
            ippStsFBankFlagErr = -70,
            ippStsFBankErr = -69,
            ippStsNegOccErr = -67,
            ippStsCdbkFlagErr = -66,
            ippStsSVDCnvgErr = -65,
            ippStsJPEGHuffTableErr = -64,
            ippStsJPEGDCTRangeErr = -63,
            ippStsJPEGOutOfBufErr = -62,
            ippStsDrawTextErr = -61,
            ippStsChannelOrderErr = -60,
            ippStsZeroMaskValuesErr = -59,
            ippStsQuadErr = -58,
            ippStsRectErr = -57,
            ippStsCoeffErr = -56,
            ippStsNoiseValErr = -55,
            ippStsDitherLevelsErr = -54,
            ippStsNumChannelsErr = -53,
            ippStsCOIErr = -52,
            ippStsDivisorErr = -51,
            ippStsAlphaTypeErr = -50,
            ippStsGammaRangeErr = -49,
            ippStsGrayCoefSumErr = -48,
            ippStsChannelErr = -47,
            ippStsToneMagnErr = -46,
            ippStsToneFreqErr = -45,
            ippStsTonePhaseErr = -44,
            ippStsTrnglMagnErr = -43,
            ippStsTrnglFreqErr = -42,
            ippStsTrnglPhaseErr = -41,
            ippStsTrnglAsymErr = -40,
            ippStsHugeWinErr = -39,
            ippStsJaehneErr = -38,
            ippStsStrideErr = -37,
            ippStsEpsValErr = -36,
            ippStsWtOffsetErr = -35,
            ippStsAnchorErr = -34,
            ippStsMaskSizeErr = -33,
            ippStsShiftErr = -32,
            ippStsSampleFactorErr = -31,
            ippStsSamplePhaseErr = -30,
            ippStsFIRMRFactorErr = -29,
            ippStsFIRMRPhaseErr = -28,
            ippStsRelFreqErr = -27,
            ippStsFIRLenErr = -26,
            ippStsIIROrderErr = -25,
            ippStsDlyLineIndexErr = -24,
            ippStsResizeFactorErr = -23,
            ippStsInterpolationErr = -22,
            ippStsMirrorFlipErr = -21,
            ippStsMoment00ZeroErr = -20,
            ippStsThreshNegLevelErr = -19,
            ippStsThresholdErr = -18,
            ippStsContextMatchErr = -17,
            ippStsFftFlagErr = -16,
            ippStsFftOrderErr = -15,
            ippStsStepErr = -14,
            ippStsScaleRangeErr = -13,
            ippStsDataTypeErr = -12,
            ippStsOutOfRangeErr = -11,
            ippStsDivByZeroErr = -10,
            ippStsMemAllocErr = -9,
            ippStsNullPtrErr = -8,
            ippStsRangeErr = -7,
            ippStsSizeErr = -6,
            ippStsBadArgErr = -5,
            ippStsNoMemErr = -4,
            ippStsSAReservedErr3 = -3,
            ippStsErr = -2,
            ippStsSAReservedErr1 = -1,
            ippStsNoErr = 0,
            ippStsNoOperation = 1,
            ippStsMisalignedBuf = 2,
            ippStsSqrtNegArg = 3,
            ippStsInvZero = 4,
            ippStsEvenMedianMaskSize = 5,
            ippStsDivByZero = 6,
            ippStsLnZeroArg = 7,
            ippStsLnNegArg = 8,
            ippStsNanArg = 9,
            ippStsJPEGMarker = 10,
            ippStsResFloor = 11,
            ippStsOverflow = 12,
            ippStsLSFLow = 13,
            ippStsLSFHigh = 14,
            ippStsLSFLowAndHigh = 15,
            ippStsZeroOcc = 16,
            ippStsUnderflow = 17,
            ippStsSingularity = 18,
            ippStsDomain = 19,
            ippStsNonIntelCpu = 20,
            ippStsCpuMismatch = 21,
            ippStsNoIppFunctionFound = 22,
            ippStsDllNotFoundBestUsed = 23,
            ippStsNoOperationInDll = 24,
            ippStsInsufficientEntropy = 25,
            ippStsOvermuchStrings = 26,
            ippStsOverlongString = 27,
            ippStsAffineQuadChanged = 28,
            ippStsWrongIntersectROI = 29,
            ippStsWrongIntersectQuad = 30,
            ippStsSmallerCodebook = 31,
            ippStsSrcSizeLessExpected = 32,
            ippStsDstSizeLessExpected = 33,
            ippStsStreamEnd = 34,
            ippStsDoubleSize = 35,
            ippStsNotSupportedCpu = 36,
            ippStsUnknownCacheSize = 37,
            ippStsSymKernelExpected = 38,
            ippStsEvenMedianWeight = 39,
            ippStsWrongIntersectVOI = 40,
            ippStsI18nMsgCatalogInvalid = 41,
            ippStsI18nGetMessageFail = 42,
            ippStsWaterfall = 43,
            ippStsPrevLibraryUsed = 44,
            ippStsLLADisabled = 45,
            ippStsNoAntialiasing = 46,
            ippStsRepetitiveSrcData = 47,
        };
        public enum IppHintAlgorithm
        {
            ippAlgHintNone = 0,
            ippAlgHintFast = 1,
            ippAlgHintAccurate = 2,
        };
        public enum IppAlgType : ulong
        {
            IppAlgAuto = 0,
            IppAlgDirect = 1,
            IppAlgFFT = 2,
            IppAlgMask = 255
        };
        public enum IppsNormOp : ulong
        {
            IppsNormNone = 0,
            IppsNormA = 256,
            IppsNormB = 512,
            IppsNormMask = 65280
        }
        public enum IppWinType
        {
            ippWinBartlett = 0,
            ippWinBlackman = 1,
            ippWinHamming = 2,
            ippWinHann = 3,
            ippWinRect = 4,
        };
        public enum IppRoundMode
        {
            ippRndZero,
            ippRndNear,
            ippRndFinancial
        }
        public struct ippiSize
        {
            public int width;
            public int height;
            public ippiSize(int width, int height)
            {
                this.width = width;
                this.height = height;
            }
        }
        public struct ippiPoint
        {
            public int width;
            public int height;
            public ippiPoint(int width, int height)
            {
                this.width = width;
                this.height = height;
            }
        }
        public enum IppiInterpolationType
        {
            ippNearest = 1,
            ippLinear = 2,
            ippCubic = 5,
            ippLanczos = 16,
            ippHahn = 0,
            ippSuper = 8
        }
        public enum IppiBorderType
        {
            ippBorderRepl = 1,
            ippBorderWrap = 2,
            ippBorderMirror = 3, /* left border: 012... -> 21012... */
            ippBorderMirrorR = 4, /* left border: 012... -> 210012... */
            ippBorderDefault = 5,
            ippBorderConst = 6,
            ippBorderTransp = 7,

            /* Flags to use source image memory pixels from outside of the border in particular directions */
            ippBorderInMemTop = 0x0010,
            ippBorderInMemBottom = 0x0020,
            ippBorderInMemLeft = 0x0040,
            ippBorderInMemRight = 0x0080,
            ippBorderInMem = ippBorderInMemLeft | ippBorderInMemTop | ippBorderInMemRight | ippBorderInMemBottom,

            /* Flags to use source image memory pixels from outside of the border for first stage only in multi-stage filters */
            ippBorderFirstStageInMemTop = 0x0100,
            ippBorderFirstStageInMemBottom = 0x0200,
            ippBorderFirstStageInMemLeft = 0x0400,
            ippBorderFirstStageInMemRight = 0x0800,
            ippBorderFirstStageInMem = ippBorderFirstStageInMemLeft | ippBorderFirstStageInMemTop | ippBorderFirstStageInMemRight | ippBorderFirstStageInMemBottom
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IppsFFTSpec_C_32fc { };
        [StructLayout(LayoutKind.Sequential)]
        public struct IppsDFTSpec_C_32fc { };
        public enum FFT_flag
        {
            IPP_FFT_DIV_FWD_BY_N = 1,
            IPP_FFT_DIV_INV_BY_N = 2,
            IPP_FFT_DIV_BY_SQRTN = 4,
            IPP_FFT_NODIV_BY_ANY = 8
        };
        public unsafe class dll_import
        {
            [DllImport(libname)]
            public static extern
            byte* ippsMalloc_8u(int len);
            [DllImport(libname)]
            public static extern
            float* ippsMalloc_32f(int len);
            [DllImport(libname)]
            public static extern
            int* ippMalloc(int len);
            [DllImport(libname)]
            public static extern
            double* ippsMalloc_64f(int len);

            [DllImport(libname)]
            public static extern
            void ippsFree(void* ptr);

            [DllImport(libname)]
            public static extern
            void ippFree(void* ptr);
            #region Resample
            /*
             *  inRate       The input rate for fixed factor resampling.
                outRate      The output rate for fixed factor resampling.
                len          The filter length for fixed factor resampling.
                                The pointer to the variable that contains the size of
                                the polyphase resampling structure.
                pSize        The pointer to the variable that contains the real filter
                                length.
                pLen         The pointer to the variable that contains the number
                                of filters.
                pHeight      Suggests using specific code (must be equal to
                                 ippAlgHintFast). The possible values for the
                                 parameter hint are given in Hint Arguments.
                hint         pSpec The pointer to the resampling state structure.
                pSrc         The pointer to the input vector.
                pDst         The pointer to the output vector.
                len          The number of input vector elements to resample.
                norm         The norm factor for output samples.
                factor       The resampling factor.
                pTime        The pointer to the start time of resampling (in input
                                 vector elements). Keeps the input sample number
                                 and the phase for the first output sample from the
                                 next input data portion.
                pOutlen      The number of calculated output vector elements.
            */

            #region Image
            [DllImport(libname)]
            public static extern
                  IppStatus ippiResizeGetSize_8u(ippiSize srcSize, ippiSize dstSize, IppiInterpolationType ippLanczos, uint antialiasing, int* specSize, int* initSize);
            [DllImport(libname)]
            public static extern
      IppStatus ippiResizeLanczosInit_8u(ippiSize srcSize, ippiSize dstSize, uint numLobes, byte* pSpec, byte* pInitBuf);
            [DllImport(libname)]
            public static extern
      IppStatus ippiResizeGetBufferSize_8u(byte* pSpec, ippiSize dstSize, uint numChannels, int* pBufSize);
            [DllImport(libname)]
            public static extern
      IppStatus ippiResizeLanczos_8u_C3R(byte* pSrc, int srcStep, byte* pDst, int dstStep, ippiPoint dstOffset,
                ippiSize dstSize, IppiBorderType border,byte* sd, byte* pSpec, byte* pBuffer);



            #endregion
            [DllImport(libname)]
            public static extern
            IppStatus ippsResamplePolyphaseFixed_32f(float* pSrc, int len, float* pDst, float norm, double* pTime
                , int* pOutlen, byte* pSpec);

            [DllImport(libname)]
            public static extern
            IppStatus ippsResamplePolyphaseFixedInit_32f(int inRate, int outRate, int len, float rollf, float alpha
                , byte* State, IppHintAlgorithm hint);

            [DllImport(libname)]
            public static extern
            IppStatus ippsResamplePolyphaseFixedGetSize_32f(int inRate, int outRate, int len, int* pSize, int* pLen,
                 int* pHeight, IppHintAlgorithm hint);
            #endregion
            #region convolve
            /*
             *  pSrc1, pSrc2     Pointers to the two vectors to be convolved.
                src1Len          Number of elements in the vector pSrc1.
                src2Len          Number of elements in the vector pSrc2.
                pDst             Pointer to the vector pDst. This vector stores the result
                                     of the convolution.
                dstLen           Number of elements in the vector pDst.
                bias             Parameter that specifes the starting element of the
                                    convolution.
             */
            [DllImport(libname)]
            public static extern
            IppStatus ippsConvBiased_32f(float* pSrc1, int inLen1, float* pSrc2, int inLen2, float* pDest, int desLen, int bias);
            #endregion
            [DllImport(libname)]
            public static extern
            IppStatus ippsMean_32f(float* pSrc, int len, float* pMean,
            IppHintAlgorithm hint);
            #region LZSS
            [DllImport(libname)]
            public static extern
            IppStatus ippsLZSSGetSize_8u(int* pLZSSStateSize);
            [DllImport(libname)]
            public static extern
            IppStatus ippsEncodeLZSSInit_8u(byte* pLZSSState);
            [DllImport(libname)]
            public static extern
            IppStatus ippsEncodeLZSS_8u(byte** ppSrc, int* pSrcLen, byte** ppDst, int* pDstLen, byte* pLZSSState);
            [DllImport(libname)]
            public static extern
            IppStatus ippsEncodeLZSSFlush_8u(byte* ppDst, int* pDstLen, byte* pLZSSState);
            [DllImport(libname)]
            public static extern
            IppStatus ippsDecodeLZSSInit_8u(byte* pLZSSState);
            [DllImport(libname)]
            public static extern
            IppStatus ippsDecodeLZSS_8u(byte* ppSrc, int* pSrcLen, byte* ppDst, int* pDstLen, byte* pLZSSState);
            #endregion
            #region goertzel
            /*
             *  pSrc     Pointer to the input complex data vector.
                len      Number of elements in the vector.
                pVal     Pointer to the output DFT value.
                rFreq    Single relative frequency value [0, 1.0)
            */
            [DllImport(libname)]
            public static extern
            IppStatus ippsGoertz_32f(float* pSrc, int Len, ComplexF* freqABS, float relative_frequency);
            #endregion

            #region correlation
            [DllImport(libname,CallingConvention = CallingConvention.Cdecl)]
            public static extern
            IppStatus ippsAutoCorrNormGetBufferSize(int SrcLen, int destLen, IppDataType IDT
               , IppAlgType hint, int* buffSize);
            [DllImport(libname)]
            public static extern
           IppStatus ippsCrossCorrNormGetBufferSize(int Src1Len, int Src2Len, int destLen, int lowLag, IppDataType IDT
              , int hint, int* buffSize);

            [DllImport(libname)]
            public static extern
            IppStatus ippsAutoCorrNorm_32f(float* pSrc, int srcLen, float* pDst, int
                dstLen, int hint, byte* buff);
            [DllImport(libname)]
            public static extern
           IppStatus ippsAutoCorr_NormA_32f(float* pSrc, int srcLen, float*
                pDst, int dstLen);
            [DllImport(libname)]
            public static extern
        IppStatus ippsAutoCorr_NormB_32f(float* pSrc, int srcLen, float*
             pDst, int dstLen);
            [DllImport(libname)]
            public static extern
      IppStatus ippsAutoCorrNorm_32fc(ComplexF* pSrc, int srcLen, ComplexF* pDst,
            int dstLen, int hint, byte* buff);
            [DllImport(libname)]
            public static extern
      IppStatus ippsAutoCorr_NormA_32fc(ComplexF* pSrc, int srcLen, ComplexF* pDst,
            int dstLen);
            [DllImport(libname)]
            public static extern
     IppStatus ippsAutoCorr_NormB_32fc(ComplexF* pSrc, int srcLen, ComplexF* pDst,
            int dstLen);
            [DllImport(libname)]
            public static extern
   IppStatus ippsCrossCorrNorm_32f(float* pSrc1, int src1Len, float*
pSrc2, int src2Len, float* pDst, int dstLen, int lowLag, int hint, byte* buff);
            [DllImport(libname)]
            public static extern
   IppStatus ippsCrossCorrNorm_32fc(ComplexF* pSrc1, int src1Len, ComplexF*
pSrc2, int src2Len, ComplexF* pDst, int dstLen, int lowLag, int hint, byte* buff);
            #endregion

            [DllImport(libname)]
            public static extern
            IppStatus ippsMean_32fc(ComplexF* pSrc, int len, ComplexF* pMean, IppHintAlgorithm hint);
            [DllImport(libname)]
            public static extern
            IppStatus ippsCopy_32fc(ComplexF* pSrc, ComplexF* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsCopy_32f(float* pSrc, float* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsCopy_16s(short* pSrc, short* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsCopy_8u(byte* pSrc, byte* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMagnitude_32fc(ComplexF* pSrc, float* pDst, int len);

            [DllImport(libname)]
            public static extern
            IppStatus ippsPowerSpectr_32fc(ComplexF* pSrc, float* pDst, int len);

            [DllImport(libname)]
            public static extern
            IppStatus ippsDivC_32fc_I(ComplexF val, ComplexF* pSrcDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsDivC_32f_I(float val, float* pSrcDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsAdd_32fc_I(ComplexF* pSrc, ComplexF* pSrcDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsAdd_32fc(ComplexF* pSrc1, ComplexF* pSrc2, ComplexF* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsAdd_32f_I(float* pSrc, float* pSrcDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsAdd_32f(float* pSrc1, float* pSrc2, float* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsAddC_32f_I(float val, float* pSrcDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsSum_32f(float* pSrc, int len, float* pSum, IppHintAlgorithm hint);
            [DllImport(libname)]
            public static extern
            IppStatus ippsSum_32fc(ComplexF* pSrc, int len, ComplexF* pSum, IppHintAlgorithm hint);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMul_32f32fc_I(float* pSrc, ComplexF* pSrcDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMul_32f32fc(float* pSrc1, ComplexF* pSrc2, ComplexF* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMulC_16sc_ISfs(ComplexS val, ComplexS* pSrcDst, int len, int scaleFactor);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMulC_32f_I(float val, float* pSrc, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMul_32f(float* pSrc1, float* pSrc2, float* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsConj_32fc_I(ComplexF* pSrcDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsConj_16sc_I(ComplexS* pSrcDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMulByConj_32fc_A21(ComplexF* pSrc1, ComplexF* pSrc2, ComplexF* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsConjFlip_32fc(ComplexF* pSrcDst, ComplexF* pDst, int len);
            #region Atan
            [DllImport(libname)]
            public static extern
            IppStatus ippsArctan_32f_I(float* pSrcDst, int len);

            [DllImport(libname)]
            public static extern
            IppStatus ippsArctan_32f(float* pSrc, float* pDst, int len);
            #endregion
            #region Atan2
            [DllImport(libname)]
            public static extern
                IppStatus ippsAtan2_32f_A11(float* pSrc1, float* pSrc2, float* pDst, int len);
            [DllImport(libname)]
            public static extern
                IppStatus ippsAtan2_32f_A21(float* pSrc1, float* pSrc2, float* pDst, int len);
            [DllImport(libname)]
            public static extern
                IppStatus ippsAtan2_32f_A24(float* pSrc1, float* pSrc2, float* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsPhase_32fc(ComplexF* pSrc, ComplexF* pDst, int len);
            #endregion
            #region Mul
            [DllImport(libname)]
            public static extern
            IppStatus ippsMul_32fc_A11(ComplexF* pSrc1, ComplexF* pSrc2, ComplexF* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMulC_32fc(ComplexF* pSrc, ComplexF val, ComplexF* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMulC_32fc_I(ComplexF val, ComplexF* pSrc, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMul_32fc_I(ComplexF* pSrc, ComplexF* pSrcDst, int len);
            #endregion
            #region FFT
            [DllImport(libname)]
            public static extern
            IppStatus ippsFFTGetSize_C_32fc(int order, int flag, IppHintAlgorithm hint,
                int* pSpecSize, int* pSpecBufferSize, int* pBufferSize);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFFTInit_C_32fc(IppsFFTSpec_C_32fc** ppFFTSpec, int order, int
                flag, IppHintAlgorithm hint, byte* pSpec, byte* pSpecBuffer);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFFTGetBufSize_C_32fc(IppsFFTSpec_C_32fc* pFFTSpec, int* pBufferSize);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFFTFwd_CToC_32fc(ComplexF* pSrc, ComplexF* pDst, IppsFFTSpec_C_32fc* pFFTSpec, byte* pBuffer);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFFTFwd_CToC_32fc_I(ComplexF* pSrcDst, IppsFFTSpec_C_32fc* pFFTSpec, byte* pBuffer);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFFTInv_CToC_32fc(ComplexF* pSrc, ComplexF* pDst,
                IppsFFTSpec_C_32fc* pFFTSpec, byte* pBuffer);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFFTInv_CToC_32fc_I(ComplexF* pSrcDst, IppsFFTSpec_C_32fc* pFFTSpec, byte* pBuffer);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFFTFree_C_32fc(IppsFFTSpec_C_32fc* pFFTSpec);
            #endregion
            #region DFT
            [DllImport(libname)]
            public static extern
            IppStatus ippsDFTInit_C_32fc(int len, int flag, IppHintAlgorithm hint, IppsDFTSpec_C_32fc* ppDFTSpec, byte* pMemInit);
            [DllImport(libname)]
            public static extern
            IppStatus ippsDFTGetSize_C_32fc(int len, int flag, IppHintAlgorithm hint,
                int* pSpecSize, int* pSpecBufferSize, int* pBufferSize);
            [DllImport(libname)]
            public static extern
            IppStatus ippsDFTFwd_CToC_32fc(ComplexF* pSrc, ComplexF* pDst, IppsDFTSpec_C_32fc* pFFTSpec, byte* pBuffer);
            [DllImport(libname)]
            public static extern
            IppStatus ippsDFTInv_CToC_32fc(ComplexF* pSrc, ComplexF* pDst, IppsDFTSpec_C_32fc* pFFTSpec, byte* pBuffer);
            #endregion
            #region Filter
            [DllImport(libname)]
            public static extern
            IppStatus ippsFIRGenLowpass_64f(double rFreq, double* taps, int tapsLen,
                IppWinType winType, bool doNormal, byte* pBuffer);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFIRGenHighpass_64f(double rFreq, double* taps, int tapsLen,
                IppWinType winType, bool doNormal, byte* pBuffer);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFIRGenBandpass_64f(double rLowFreq, double rHighFreq, double* taps, int tapsLen,
                IppWinType winType, bool doNormal, byte* pBuffer);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFIRGenBandstop_64f(double rLowFreq, double rHighFreq, double* taps, int tapsLen,
                IppWinType winType, bool doNormal, byte* pBuffer);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFIRGenGetBufferSize(int tapsLen, int* pBuffer);
            #endregion
            #region bitwise operation
            [DllImport(libname)]
            public static extern
            IppStatus ippsXor_8u(byte* pSrc1, byte* pSrc2, byte* pDst, int len);
            [DllImport(libname)]
            public static extern
              IppStatus ippsAnd_8u(byte* pSrc1, byte* pSrc2, byte* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsAndC_8u_I(byte val, byte* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsSubC_8u_ISfs(byte val, byte* pSrcDst, int len, int scaleFactor);

            [DllImport(libname)]
            public static extern
            IppStatus ippsNot_8u_I(byte* pSrcDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFlip_32fc_I(ComplexF* pSrcDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFlip_32fc(ComplexF* pSrc, ComplexF* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFlip_32f_I(float* pSrcDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsFlip_32f(float* pSrc, float* pDst, int len);
            #endregion
            [DllImport(libname)]
            public static extern
            IppStatus ippsRealToCplx_32f(float* pSrcRe, float* pSrcIm,
                ComplexF* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsReal_32fc(ComplexF* pSrc, float* pDstRe, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsImag_32fc(ComplexF* pSrc, float* pDstRe, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsPowx_32fc_A21(ComplexF* pSrc1, ComplexF ConstValue, ComplexF* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMax_32f(float* pSrc1, int len, float* max);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMin_32f(float* pSrc1, int len, float* min);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMaxIndx_32f(float* pSrc1, int len, ref float max, ref int ind);
            [DllImport(libname)]
            public static extern
            IppStatus ippsMinIndx_32f(float* pSrc1, int len, ref float min, ref int ind);
            [DllImport(libname)]
            public static extern
            IppStatus ippsPowx_32fc_A21(ComplexF* pSrc1, float ConstValue, ComplexF* pDst, int len);

            [DllImport(libname)]
            public static extern
            IppStatus ippsConvert_32f16s_Sfs(float* pSrc, short* pDst, int len, IppRoundMode rndMode, int scaleFactor);
            [DllImport(libname)]
            public static extern
            IppStatus ippsConvert_16s32f_Sfs(float* pSrc, short* pDst, int len, IppRoundMode rndMode, int scaleFactor);
            [DllImport(libname)]
            public static extern
            IppStatus ippsConvert_16u32f(short* pSrc, float* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsConvert_16s32f(short* pSrc, float* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsConvert_8s32f(byte* pSrc, float* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsConvert_8u32f(byte* pSrc, float* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsConvert_8s16s(byte* pSrc, short* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsConvert_24s32s(byte* pSrc, uint* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsConvert_64f32f(double* pSrc, float* pDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsSwapBytes_16u_I(short* pSrcDst, int len);
            [DllImport(libname)]
            public static extern
            IppStatus ippsLog10_32f_A11(float* pSrc, float* pDst, int len);

            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsFilterNoiseGetStateSize_EC_32f(int freq, int* len);
            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsFilterNoiseGetStateSize_RTA_32f(int freq, int* len);
            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsFilterNoiseInit_EC_32f(int freq, float* pNRStateMem);
            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsFilterNoiseInit_RTA_32f(int freq, float* pNRStateMem);
            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsFilterNoiseLevel_EC_32f(IppsNRLevel level, float* pNRStateMem);
            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsFilterNoiseLevel_RTA_32f(IppsNRLevel level, float* pNRStateMem);
            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsFilterNoiseDetect_EC_32f64f(float* pSrc, double* pNoisePower, float* pMean, int* pNoiseFlag, float* pNRStateMem);
            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsFilterNoiseSetMode_EC_32f(IppsNrSmoothMode mode, float* pNRStateMem);
            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsFilterNoise_EC_32f_I(float* Signal,
                IppsNrMode filterMode, float* pNRStateMem);
            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsFilterNoise_RTA_32f_I(float* Signal, float* pNRStateMem);

            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsVADGetSize_AMRWB_16s(int* pDstSize);
            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsVADInit_AMRWB_16s(short* State);
            [DllImport(libname_legacy)]
            public static extern
            IppStatus legacy90ippsVAD_AMRWB_16s(short* pSrcSpch, short* pSrcDstVadState
                                              , short* pToneFlag, short* pVadFlag);


        }
        public unsafe class Image_resize
        {
            public int resizeExample_C3R(ref byte[] Src, ippiSize srcSize, int srcStep, ref byte[] Dst, ippiSize dstSize, int dstStep)
            {
                fixed (byte* pSrc = Src, pDst = Dst) {
                    byte* pSpec;
                    int specSize = 0, initSize = 0, bufSize = 0;
                    byte* pBuffer;
                    byte* pInitBuf;
                    uint numChannels = 3;
                    ippiPoint dstOffset = new ippiPoint();
                    dstOffset.width = 0; dstOffset.height = 0;
                    int status = 0;
                    IppiBorderType border = IppiBorderType.ippBorderRepl;

                    /* Spec and init buffer sizes */
                    status = (int)dll_import.ippiResizeGetSize_8u(srcSize, dstSize, IppiInterpolationType.ippLanczos, 0, &specSize, &initSize);

                    if (status != 0) return status;

                    /* Memory allocation */
                    pInitBuf = dll_import.ippsMalloc_8u(initSize);
                    pSpec = dll_import.ippsMalloc_8u(specSize);

                    if (pInitBuf == (byte*)0 || pSpec == (byte*)0)
                    {
                        dll_import.ippsFree(pInitBuf);
                        dll_import.ippsFree(pSpec);
                        return (int)IppStatus.ippStsNoMemErr;
                    }

                    /* Filter initialization */
                    status = (int)dll_import.ippiResizeLanczosInit_8u(srcSize, dstSize, 3, pSpec, pInitBuf);
                    dll_import.ippsFree(pInitBuf);

                    if (status != 0)
                    {
                        dll_import.ippsFree(pSpec);
                        return status;
                    }

                    /* work buffer size */
                    status = (int)dll_import.ippiResizeGetBufferSize_8u(pSpec, dstSize, numChannels, &bufSize);
                    if (status != 0)
                    {
                        dll_import.ippsFree(pSpec);
                        return status;
                    }

                    pBuffer = dll_import.ippsMalloc_8u(bufSize);
                    if (pBuffer == (byte*)0)
                    {
                        dll_import.ippsFree(pSpec);
                        return (int)IppStatus.ippStsNoMemErr;
                    }

                    /* Resize processing */
                    byte* tBuffer = dll_import.ippsMalloc_8u(4);
                    status = (int)dll_import.ippiResizeLanczos_8u_C3R(pSrc, srcStep, pDst, dstStep, dstOffset, dstSize, border, tBuffer, pSpec, pBuffer);



                    dll_import.ippsFree(pSpec);
                    dll_import.ippsFree(pBuffer);

                    return status;
                }
            }
        }
        public unsafe class Resampler
        {
            #region Variable
            private byte* state;
            private double time;
            public int outLen;
            private int size;
            private int len;
            private int height;
            public IppHintAlgorithm Algorithm;
            public int inRate, outRate, History_len;
            public float rollf, alpha;
            public int Status;
            #endregion

            ~Resampler()
            {
                dll_import.ippsFree(state);
                state = null;
            }

            public unsafe int Init(int inRate, int outRate, int History_len, float rollf, float alpha, IppHintAlgorithm Alg)
            {
                this.inRate = inRate; this.outRate = outRate;
                this.History_len = History_len;
                History_len = (2 * this.History_len - 1) < 1 ? 1 : (2 * this.History_len - 1);
                this.rollf = rollf; this.alpha = alpha;
                this.Algorithm = Alg;
                fixed (int* psize = &this.size, plen = &this.len, pheight = &this.height)
                    Status = (int)dll_import.ippsResamplePolyphaseFixedGetSize_32f(this.inRate, this.outRate, History_len, psize, plen, pheight, this.Algorithm);
                if (Status == 0)
                {
                    state = dll_import.ippsMalloc_8u(size);
                    if (state == null)
                        return (int)IppStatus.ippStsMemAllocErr;
                    Status = (int)dll_import.ippsResamplePolyphaseFixedInit_32f(this.inRate, this.outRate, History_len, this.rollf, this.alpha, state, this.Algorithm);
                    return Status;
                }
                else
                    return Status;
            }
            public unsafe int StartResampling(ref float[] inBuff, ref float[] outBuff, int startIndex, int length, float norm)
            {
                time = History_len; outLen = 0;
                fixed (double* ptime = &time)
                fixed (int* poutLen = &outLen)
                fixed (float* pin = inBuff, pout = outBuff)
                    return (int)dll_import.ippsResamplePolyphaseFixed_32f(pin, length, pout, norm, ptime, poutLen, state);
            }
        }
        public class Convolution
        {
            public unsafe int Convolve(ref float[] sig1, int from1, int len1, ref float[] sig2, int from2, int len2, ref float[] convOut, int from, int len, int numDiscardedCalcedElement)
            {
                fixed (float* psig1 = sig1, psig2 = sig2, pconvout = convOut)
                    return (int)dll_import.ippsConvBiased_32f(psig1 + from1, len1, psig2 + from2, len2, pconvout + from, len, numDiscardedCalcedElement);
            }

        }
        public class AutoCorrelation
        {
            public unsafe int Convolve(ref float[] sig1, int from1, int len1, ref float[] sig2, int from2, int len2, ref float[] convOut, int from, int len, int numDiscardedCalcedElement)
            {
                fixed (float* psig1 = sig1, psig2 = sig2, pconvout = convOut)
                    return (int)dll_import.ippsConvBiased_32f(psig1 + from1, len1, psig2 + from2, len2, pconvout + from, len, numDiscardedCalcedElement);
            }

        }
        public class Goertzel
        {
            public int Status;
            public ComplexF tmp;
            public unsafe ComplexF Goertzel_f(ref float[] signal, int from, int len, float relative_frequency)
            {
                fixed (float* psignal = signal)
                fixed (ComplexF* pfreqABS = &tmp)
                    Status = (int)dll_import.ippsGoertz_32f(psignal + from, len, pfreqABS, relative_frequency);
                return tmp;
            }
        }
        public class Mean
        {
            private float mean_tmp;
            private ComplexF mean_tmp_c;
            public IppHintAlgorithm Mean_hint_tmp = IppHintAlgorithm.ippAlgHintAccurate;
            public int Mean_Status;
            public unsafe float mean(ref float[] input)
            {
                fixed (float* pinput = input)
                fixed (float* pmean_tmp = &mean_tmp)
                    Mean_Status = (int)dll_import.ippsMean_32f(pinput, input.Length, pmean_tmp, Mean_hint_tmp);
                return mean_tmp;
            }
            public unsafe float mean(ref float[] input, int from, int len)
            {
                fixed (float* pinput = input)
                fixed (float* pmean_tmp = &mean_tmp)
                    Mean_Status = (int)dll_import.ippsMean_32f(pinput + from, len, pmean_tmp, Mean_hint_tmp);
                return mean_tmp;
            }
            public unsafe ComplexF mean(ref ComplexF[] input)
            {
                fixed (ComplexF* pinput = input)
                fixed (ComplexF* pmean_tmp = &mean_tmp_c)
                    Mean_Status = (int)dll_import.ippsMean_32fc(pinput, input.Length, pmean_tmp, Mean_hint_tmp);
                return mean_tmp_c;
            }
            public unsafe ComplexF mean(ref ComplexF[] input, int from, int len)
            {
                fixed (ComplexF* pinput = input)
                fixed (ComplexF* pmean_tmp = &mean_tmp_c)
                    Mean_Status = (int)dll_import.ippsMean_32fc(pinput + from, len, pmean_tmp, Mean_hint_tmp);
                return mean_tmp_c;
            }
        }
        public class Sum
        {
            private float Sum_tmp;
            private ComplexF Sum_tmp_C;
            public IppHintAlgorithm Sum_hint_tmp = IppHintAlgorithm.ippAlgHintAccurate;
            public int Sum_Status;
            public unsafe float sum(ref float[] input)
            {
                fixed (float* pinput = input)
                fixed (float* pSum_tmp = &Sum_tmp)
                    Sum_Status = (int)dll_import.ippsSum_32f(pinput, input.Length, pSum_tmp, Sum_hint_tmp);
                return Sum_tmp;
            }
            public unsafe float sum(ref float[] input, int from, int len)
            {
                fixed (float* pinput = input)
                fixed (float* pSum_tmp = &Sum_tmp)
                    Sum_Status = (int)dll_import.ippsSum_32f(pinput + from, len, pSum_tmp, Sum_hint_tmp);
                return Sum_tmp;
            }
            public unsafe ComplexF sum(ref ComplexF[] input)
            {
                fixed (ComplexF* pinput = input)
                fixed (ComplexF* pSum_tmp = &Sum_tmp_C)
                    Sum_Status = (int)dll_import.ippsSum_32fc(pinput, input.Length, pSum_tmp, Sum_hint_tmp);
                return Sum_tmp_C;
            }
            public unsafe ComplexF sum(ref ComplexF[] input, int from, int len)
            {
                fixed (ComplexF* pinput = input)
                fixed (ComplexF* pSum_tmp = &Sum_tmp_C)
                    Sum_Status = (int)dll_import.ippsSum_32fc(pinput + from, len, pSum_tmp, Sum_hint_tmp);
                return Sum_tmp_C;
            }
        }
        public class Atan
        {
            public int atan_Status;
            public unsafe void atan(ref float input)
            {
                fixed (float* pinput = &input)
                    atan_Status = (int)dll_import.ippsArctan_32f_I(pinput, 1);
            }
            public unsafe void atan(ref float[] input, int len)
            {
                fixed (float* pinput = input)
                    atan_Status = (int)dll_import.ippsArctan_32f_I(pinput, len);
            }
            public unsafe float atan(float input)
            {
                atan_Status = (int)dll_import.ippsArctan_32f_I(&input, 1);
                return input;
            }
            public unsafe void atan(ref float[] input, int fromIn, ref float[] output, int fromOut, int len)
            {
                fixed (float* pinput = input, poutput = output)
                    atan_Status = (int)dll_import.ippsArctan_32f(pinput + fromIn, poutput + fromOut, len);
            }
        }
        public class Atan2
        {
            public int atan2_Status;
            public float angle;
            public unsafe float atan2(ref float y, ref float x)
            {
                fixed (float* py = &y, px = &x)
                fixed (float* pangle = &angle)
                    atan2_Status = (int)dll_import.ippsAtan2_32f_A11(py, px, pangle, 1);
                return angle;
            }
            public unsafe float atan2(ref ComplexF input)
            {
                fixed (ComplexF* pinput = &input)
                fixed (float* pangle = &angle)
                    atan2_Status = (int)dll_import.ippsAtan2_32f_A11((float*)pinput, ((float*)pinput) + 4, pangle, 1);
                return angle;
            }
            public unsafe void atan2(ref ComplexF[] input, ref float[] output, int Len)
            {
                fixed (ComplexF* pinput = input)
                fixed (float* pangle = &angle)
                fixed (float* poutput = output)
                    atan2_Status = (int)dll_import.ippsAtan2_32f_A11((float*)pinput, ((float*)pinput) + 4, poutput, Len);
            }
        }
        public class Mul
        {
            public int mul_Status;
            public unsafe void mul(ref ComplexF input1, ref ComplexF input2, ref ComplexF output)
            {
                fixed (ComplexF* pinput1 = &input1, pinput2 = &input2, poutput = &output)
                    mul_Status = (int)dll_import.ippsMul_32fc_A11(pinput1, pinput2, poutput, 1);
            }
            public unsafe void mul(ref ComplexF[] input1, int from1, ref ComplexF[] input2, int from2, ref ComplexF[] output, int from3, int Len)
            {
                fixed (ComplexF* pinput1 = input1, pinput2 = input2, poutput = output)
                    mul_Status = (int)dll_import.ippsMul_32fc_A11(pinput1+ from1, pinput2+ from2, poutput+ from3, Len);
            }

            public unsafe void mul(ref ComplexF[] input, ref ComplexF[] in_out)
            {
                fixed (ComplexF* pinput1 = input, pin_out = in_out)
                    mul_Status = (int)dll_import.ippsMul_32fc_I(pinput1, pin_out, input.Length);
            }
            public unsafe void mul(ref ComplexF[] input, int from1, ref ComplexF[] in_out, int from2, int len)
            {
                fixed (ComplexF* pinput1 = input, pin_out = in_out)
                    mul_Status = (int)dll_import.ippsMul_32fc_I(pinput1 + from1, pin_out + from2, len);
            }

            public unsafe void mul(ref ComplexF[] input, ComplexF Scale, ref ComplexF[] output)
            {
                fixed (ComplexF* pinput1 = input, poutput = output)
                    mul_Status = (int)dll_import.ippsMulC_32fc(pinput1, Scale, poutput, input.Length);
            }
            public unsafe void mul(ref ComplexF[] input, int from_in, ComplexF Scale, ref ComplexF[] output, int from_out, int len)
            {
                fixed (ComplexF* pinput1 = input, poutput = output)
                    mul_Status = (int)dll_import.ippsMulC_32fc(pinput1 + from_in, Scale, poutput + from_out, len);
            }
            public unsafe void mul(ref ComplexF[] inout, ComplexF Scale)
            {
                fixed (ComplexF* pinput1 = inout)
                    mul_Status = (int)dll_import.ippsMulC_32fc_I(Scale, pinput1, inout.Length);
            }
            public unsafe void mul(ref ComplexF[] inout, int from, ComplexF Scale, int len)
            {
                fixed (ComplexF* pinput1 = inout)
                    mul_Status = (int)dll_import.ippsMulC_32fc_I(Scale, pinput1 + from, len);
            }
            
            public unsafe void mul(ref ComplexF[] inout, ref float[] vec, int len)
            {
                fixed (ComplexF* pinOut = inout)
                fixed (float* pvec = vec)
                    mul_Status = (int)dll_import.ippsMul_32f32fc_I(pvec, pinOut, len);
            }
            public unsafe void mul(ref ComplexF[] inout, int from1, ref float[] vec, int from2, int len)
            {
                fixed (ComplexF* pinOut = inout)
                fixed (float* pvec = vec)
                    mul_Status = (int)dll_import.ippsMul_32f32fc_I(pvec + from2, pinOut + from1, len);
            }
            public unsafe void mul(ref ComplexF[] input1, ref float[] input2, ref ComplexF[] output)
            {
                fixed (ComplexF* pinput1 = input1, poutput = output)
                fixed (float* pinput2 = input2)
                    mul_Status = (int)dll_import.ippsMul_32f32fc(pinput2, pinput1, poutput, input1.Length);
            }
            public unsafe void mul(ref ComplexF[] input1, int from1, ref float[] input2, int from2, ref ComplexF[] output, int from_out, int len)
            {
                fixed (ComplexF* pinput1 = input1, poutput = output)
                fixed (float* pinput2 = input2)
                    mul_Status = (int)dll_import.ippsMul_32f32fc(pinput2 + from2, pinput1 + from1, poutput + from_out, len);
            }
            public unsafe void mul(ref float[] input, float Scale)
            {
                fixed (float* pinput1 = input)
                    mul_Status = (int)dll_import.ippsMulC_32f_I(Scale, pinput1, input.Length);
            }
            public unsafe void mul(ref float[] input, int from, int len, float Scale)
            {
                fixed (float* pinput1 = input)
                    mul_Status = (int)dll_import.ippsMulC_32f_I(Scale, pinput1 + from, len);
            }
            public unsafe void mul(ref float[] input1, int from1, ref float[] input2, int from2
                , ref float[] output, int from_out, int len, float Scale)
            {
                fixed (float* pinput1 = input1, pinput2 = input2, poutput = output)
                    mul_Status = (int)dll_import.ippsMul_32f(pinput1 + from1, pinput2 + from2, poutput + from_out, len);
            }
        }
        public unsafe class FFT
        {
            int sizeSpec, sizeInit, sizeBuffer;
            int Status;
            IppsFFTSpec_C_32fc* FFTSpec;
            byte* pMemSpec, pMemInit, pMemBuffer;
            public int size;
            IppHintAlgorithm hint;
            FFT_flag flag;
            ~FFT()
            {
                clear();
            }
            public void clear()
            {
                //dll_import.ippFree(FFTSpec);
                //FFTSpec = null;
                dll_import.ippsFree(pMemSpec);
                pMemSpec = null;
                dll_import.ippsFree(pMemBuffer);
                pMemBuffer = null;
            }
            public void reset()
            {
                clear();
                sizeSpec = 0; sizeInit = 0; sizeBuffer = 0; Status = 0;
            }
            public int init(int size, FFT_flag flag, IppHintAlgorithm hint)
            {
                if (size != this.size || hint != this.hint || flag != this.flag || Status != 0)
                {
                    if (this.size != 0)
                        reset();
                }
                else
                    return Status;
                int tmp = 1, pow2_N_Value = 0;
                while (tmp < size)
                {
                    tmp *= 2;
                    pow2_N_Value++;
                }
                if (tmp != size)
                    return (int)IppStatus.ippStsContextMatchErr;
                this.size = size;
                this.hint = hint;
                this.flag = flag;
                fixed (int* pSpecSize = &sizeSpec, pSpecBufferSize = &sizeInit, pBufferSize = &sizeBuffer)
                    Status = (int)dll_import.ippsFFTGetSize_C_32fc(pow2_N_Value, (int)flag, hint, pSpecSize, pSpecBufferSize, pBufferSize);
                if (Status == 0)
                {
                    pMemSpec = dll_import.ippsMalloc_8u(sizeSpec);
                    if (pMemSpec == null)
                    {
                        Status = (int)IppStatus.ippStsMemAllocErr;
                        return Status;
                    }
                    if (sizeInit > 0)
                    {
                        pMemInit = dll_import.ippsMalloc_8u(sizeInit);
                        if (pMemInit == null)
                        {
                            Status = (int)IppStatus.ippStsMemAllocErr;
                            return Status;
                        }
                    }
                    if (sizeBuffer > 0)
                    {
                        pMemBuffer = dll_import.ippsMalloc_8u(sizeBuffer);
                        if (pMemBuffer == null)
                        {
                            Status = (int)IppStatus.ippStsMemAllocErr;
                            return Status;
                        }
                    }
                    fixed (IppsFFTSpec_C_32fc** pFFTSpec = &FFTSpec)
                        Status = (int)dll_import.ippsFFTInit_C_32fc(pFFTSpec, pow2_N_Value, (int)flag, hint, pMemSpec, pMemInit);
                    dll_import.ippsFree(pMemInit);
                    pMemInit = null;
                    return Status;
                }
                else return Status;
            }
            public int FFT_forward(ref ComplexF[] input, ref ComplexF[] output)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsFFTFwd_CToC_32fc(pinput, poutput, FFTSpec, pMemBuffer);
                return Status;
            }
            public int FFT_forward(ref ComplexF[] input, int firstIndex_in, ref ComplexF[] output, int firstIndex_out)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsFFTFwd_CToC_32fc(pinput + firstIndex_in, poutput + firstIndex_out, FFTSpec, pMemBuffer);
                return Status;
            }
            public int FFT_backward(ref ComplexF[] input, ref ComplexF[] output)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsFFTInv_CToC_32fc(pinput, poutput, FFTSpec, pMemBuffer);
                return Status;
            }
            public int FFT_backward(ref ComplexF[] input, int firstIndex_in, ref ComplexF[] output, int firstIndex_out)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsFFTInv_CToC_32fc(pinput + firstIndex_in, poutput + firstIndex_out, FFTSpec, pMemBuffer);
                return Status;
            }
            public int FFT_forward_Inplace(ref ComplexF[] input_output)
            {
                fixed (ComplexF* pinput_output = input_output)
                    Status = (int)dll_import.ippsFFTFwd_CToC_32fc_I(pinput_output, FFTSpec, pMemBuffer);
                return Status;
            }
            public int FFT_forward_Inplace(ref ComplexF[] input_output, int firstIndex)
            {
                fixed (ComplexF* pinput_output = input_output)
                    Status = (int)dll_import.ippsFFTFwd_CToC_32fc_I(pinput_output + firstIndex, FFTSpec, pMemBuffer);
                return Status;
            }
            public int FFT_backward_Inplace(ref ComplexF[] input_output)
            {
                fixed (ComplexF* pinput_output = input_output)
                    Status = (int)dll_import.ippsFFTInv_CToC_32fc_I(pinput_output, FFTSpec, pMemBuffer);
                return Status;
            }
            public int FFT_backward_Inplace(ref ComplexF[] input_output, int firstIndex)
            {
                fixed (ComplexF* pinput_output = input_output)
                    Status = (int)dll_import.ippsFFTInv_CToC_32fc_I(pinput_output + firstIndex, FFTSpec, pMemBuffer);
                return Status;
            }
        }
        public unsafe class DFT
        {
            int sizeSpec, sizeInit, sizeBuffer;
            public int Status;
            public int Size;
            IppsDFTSpec_C_32fc* DFTSpec;
            byte* pMemInit, pMemBuffer;
            IppHintAlgorithm hint;
            FFT_flag flag;
            ~DFT()
            {
                clear();
            }
            public void clear()
            {
                dll_import.ippsFree(DFTSpec);
                DFTSpec = null;
                dll_import.ippsFree(pMemBuffer);
                pMemBuffer = null;
            }
            public void reset()
            {
                clear();
                sizeSpec = 0; sizeInit = 0; sizeBuffer = 0; Status = 0;
            }
            public int init(int size, FFT_flag flag, IppHintAlgorithm hint)
            {
                if (size != this.Size || hint != this.hint || flag != this.flag || Status != 0)
                {
                    if (this.Size != 0)
                        reset();
                }
                else
                    return Status;
                Size = size;
                this.hint = hint;
                this.flag = flag;
                fixed (int* pSpecSize = &sizeSpec, pSpecBufferSize = &sizeInit, pBufferSize = &sizeBuffer)
                    Status = (int)dll_import.ippsDFTGetSize_C_32fc(size, (int)flag, hint, pSpecSize, pSpecBufferSize, pBufferSize);
                if (Status == 0)
                {
                    DFTSpec = (IppsDFTSpec_C_32fc*)dll_import.ippsMalloc_8u(sizeSpec);
                    if (DFTSpec == null)
                    {
                        Status = (int)IppStatus.ippStsMemAllocErr;
                        return Status;
                    }
                    if (sizeInit > 0)
                    {
                        pMemInit = dll_import.ippsMalloc_8u(sizeInit);
                        if (pMemInit == null)
                        {
                            Status = (int)IppStatus.ippStsMemAllocErr;
                            return Status;
                        }
                    }
                    if (sizeBuffer > 0)
                    {
                        pMemBuffer = dll_import.ippsMalloc_8u(sizeBuffer);
                        if (pMemBuffer == null)
                        {
                            Status = (int)IppStatus.ippStsMemAllocErr;
                            return Status;
                        }
                    }
                    Status = (int)dll_import.ippsDFTInit_C_32fc(size, (int)flag, hint, DFTSpec, pMemInit);
                    dll_import.ippsFree(pMemInit);
                    pMemInit = null;
                    return Status;
                }
                else return Status;
            }
            public int DFT_forward(ref ComplexF[] input, ref ComplexF[] output)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsDFTFwd_CToC_32fc(pinput, poutput, DFTSpec, pMemBuffer);
                return Status;
            }
            public int DFT_forward(ref ComplexF[] input, int firstIndex_in, ref ComplexF[] output, int firstIndex_out)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsDFTFwd_CToC_32fc(pinput + firstIndex_in, poutput + firstIndex_out, DFTSpec, pMemBuffer);
                return Status;
            }
            public int DFT_backward(ref ComplexF[] input, ref ComplexF[] output)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsDFTInv_CToC_32fc(pinput, poutput, DFTSpec, pMemBuffer);
                return Status;
            }
            public int DFT_backward(ref ComplexF[] input, int firstIndex_in, ref ComplexF[] output, int firstIndex_out)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsDFTInv_CToC_32fc(pinput + firstIndex_in, poutput + firstIndex_out, DFTSpec, pMemBuffer);
                return Status;
            }
            public int DFT_forward(ref ComplexF[] inout)
            {
                fixed (ComplexF* pinput = inout)
                    Status = (int)dll_import.ippsDFTFwd_CToC_32fc(pinput, pinput, DFTSpec, pMemBuffer);
                return Status;
            }
            public int DFT_forward(ref ComplexF[] inout, int firstIndex_in)
            {
                fixed (ComplexF* pinout = inout)
                    Status = (int)dll_import.ippsDFTFwd_CToC_32fc(pinout + firstIndex_in, pinout + firstIndex_in, DFTSpec, pMemBuffer);
                return Status;
            }
            public int DFT_backward(ref ComplexF[] inout)
            {
                fixed (ComplexF* pinout = inout)
                    Status = (int)dll_import.ippsDFTInv_CToC_32fc(pinout, pinout, DFTSpec, pMemBuffer);
                return Status;
            }
            public int DFT_backward(ref ComplexF[] inout, int firstIndex_in)
            {
                fixed (ComplexF* pinout = inout)
                    Status = (int)dll_import.ippsDFTInv_CToC_32fc(pinout + firstIndex_in, pinout + firstIndex_in, DFTSpec, pMemBuffer);
                return Status;
            }
        }
        public class Copy
        {
            public int Copy_Status;
            public unsafe void copy(ref ComplexF[] input, int from_in, int len, float* poutput, int from_out)
            {
                fixed (ComplexF* pinput = input)
                    Copy_Status = (int)dll_import.ippsCopy_32fc(pinput + from_in, ((ComplexF*)poutput) + from_out, len);
            }
            public unsafe void copy(ref ComplexF[] input, ref ComplexF[] output)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Copy_Status = (int)dll_import.ippsCopy_32fc(pinput, poutput, input.Length);
            }
            public unsafe void copy(ref ComplexF[] input, int from_in, int len, ref ComplexF[] output, int from_out)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Copy_Status = (int)dll_import.ippsCopy_32fc(pinput + from_in, poutput + from_out, len);
            }
            public unsafe void copy(ref ComplexF[] input, float* poutput)
            {
                fixed (ComplexF* pinput = input)
                    Copy_Status = (int)dll_import.ippsCopy_32fc(pinput, (ComplexF*)poutput, input.Length);
            }
            public unsafe void copy(ref float[] input, ref float[] output)
            {
                fixed (float* pinput = input, poutput = output)
                    Copy_Status = (int)dll_import.ippsCopy_32f(pinput, poutput, input.Length);
            }
            public unsafe void copy(ref float[] input, int from_in, int len, ref float[] output, int from_out)
            {
                fixed (float* pinput = input, poutput = output)
                    Copy_Status = (int)dll_import.ippsCopy_32f(pinput + from_in, poutput + from_out, len);
            }
            public unsafe void copy(ref ComplexF[] input, int from_in, int len, ref float[] output, int from_out)
            {
                fixed (ComplexF* pinput = input)
                fixed (float* poutput = output)
                    Copy_Status = (int)dll_import.ippsCopy_32f(((float*)pinput) + from_in, poutput + from_out, len);
            }
            public unsafe void copy(ref short[] input, ref short[] output)
            {
                fixed (short* pinput = input, poutput = output)
                    Copy_Status = (int)dll_import.ippsCopy_16s(pinput, poutput, input.Length);
            }
            public unsafe void copy(short* input, int from, int len, ref short[] output)
            {
                fixed (short* poutput = output)
                    Copy_Status = (int)dll_import.ippsCopy_16s(input + from, poutput, len);
            }
            public unsafe void copy(byte* input, int from, int len, ref byte[] output)
            {
                fixed (byte* poutput = output)
                    Copy_Status = (int)dll_import.ippsCopy_8u(input + from, poutput, len);
            }
            public unsafe void copy(ref byte[] input, int from, int len, byte* output, int out_from)
            {
                fixed (byte* pinput = input)
                    Copy_Status = (int)dll_import.ippsCopy_8u(pinput + from, output + out_from, len);
            }
            public unsafe void copy(ref byte[] input, int from, int len, ref byte[] output, int out_from)
            {
                fixed (byte* pinput = input, poutput = output)
                    Copy_Status = (int)dll_import.ippsCopy_8u(pinput + from, poutput + out_from, len);
            }
        }
        public class Magnitude
        {
            int Status;
            public unsafe void magnitude(ref ComplexF[] input)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsMagnitude_32fc(pinput, (float*)pinput, input.Length);
            }
            public unsafe void magnitude(ref ComplexF[] input, ref float[] output)
            {
                fixed (ComplexF* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsMagnitude_32fc(pinput, poutput, input.Length);
            }
            public unsafe void magnitude(ref ComplexF[] input, int from_in, int len, ref float[] output, int from_out)
            {
                fixed (ComplexF* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsMagnitude_32fc(pinput + from_in, poutput + from_out, len);
            }
        }
        public class MagnitudeSquered
        {
            int Status;
            public unsafe void magnitude(ref ComplexF[] input, ref float[] output)
            {
                fixed (ComplexF* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsPowerSpectr_32fc(pinput, poutput, input.Length);
            }
            public unsafe void magnitude(ref ComplexF[] input, int from_in, int len, ref float[] output, int from_out)
            {
                fixed (ComplexF* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsPowerSpectr_32fc(pinput + from_in, poutput + from_out, len);
            }
        }
        public class Division
        {
            int Status;
            public unsafe void division(ref ComplexF[] input, ComplexF valu)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsDivC_32fc_I(valu, pinput, input.Length);
            }
            public unsafe void division(ref ComplexF[] input, int from_in, int len, ComplexF valu)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsDivC_32fc_I(valu, pinput + from_in, len);
            }
            public unsafe void division(ref float[] input, float valu)
            {
                fixed (float* pinput = input)
                    Status = (int)dll_import.ippsDivC_32f_I(valu, pinput, input.Length);
            }
            public unsafe void division(ref float[] input, int from_in, int len, float valu)
            {
                fixed (float* pinput = input)
                    Status = (int)dll_import.ippsDivC_32f_I(valu, pinput + from_in, len);
            }
            public unsafe void division(ref ComplexF[] input, float valu)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsDivC_32f_I(valu, (float*)pinput, input.Length * 2);
            }
            public unsafe void division(ref ComplexF[] input, int from_in, int len, float valu)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsDivC_32f_I(valu, (float*)(pinput + from_in), len * 2);
            }
        }
        public class Add
        {
            int Status;
            public unsafe void add(ref ComplexF[] input, ref ComplexF[] output)
            {
                fixed (ComplexF* pinput = input)
                fixed (ComplexF* poutput = output)
                    Status = (int)dll_import.ippsAdd_32fc_I(pinput, poutput, input.Length);
            }
            public unsafe void add(ref ComplexF[] input, ComplexF[] output)
            {
                fixed (ComplexF* pinput = input)
                fixed (ComplexF* poutput = output)
                    Status = (int)dll_import.ippsAdd_32fc_I(pinput, poutput, input.Length);
            }
            public unsafe void add(ref ComplexF[] input, ref ComplexF[] output, int len)
            {
                fixed (ComplexF* pinput = input)
                fixed (ComplexF* poutput = output)
                    Status = (int)dll_import.ippsAdd_32fc_I(pinput, poutput, len);
            }
            public unsafe void add(ref ComplexF[] input, int from_in, int len, ref ComplexF[] output, int from_out)
            {
                fixed (ComplexF* pinput = input)
                fixed (ComplexF* poutput = output)
                    Status = (int)dll_import.ippsAdd_32fc_I(pinput + from_in, poutput + from_out, len);
            }
            public unsafe void add(ref ComplexF[] input1, ref ComplexF[] input2, ref ComplexF[] output, int len)
            {
                fixed (ComplexF* pinput1 = input1, pinput2 = input2, poutput = output)
                    Status = (int)dll_import.ippsAdd_32fc(pinput1, pinput2, poutput, len);
            }
            public unsafe void add(ref ComplexF[] input1, int from1, ref ComplexF[] input2, int from2, ref ComplexF[] output, int fromOut, int len)
            {
                fixed (ComplexF* pinput1 = input1, pinput2 = input2, poutput = output)
                    Status = (int)dll_import.ippsAdd_32fc(pinput1 + from1, pinput2 + from2, poutput + fromOut, len);
            }
            public unsafe void add(ref ComplexF[] input, float val)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsAddC_32f_I(val, (float*)pinput, input.Length * 2);
            }
            public unsafe void add(ref ComplexF[] input, float val, int len)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsAddC_32f_I(val, (float*)pinput, len);
            }
            public unsafe void add(ref ComplexF[] input, int from, int len, float val)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsAddC_32f_I(val, (float*)(pinput + from), len);
            }
            public unsafe void add(ref float[] input, float val)
            {
                fixed (float* pinput = input)
                    Status = (int)dll_import.ippsAddC_32f_I(val, pinput, input.Length);
            }
            public unsafe void add(ref float[] input, float val, int len)
            {
                fixed (float* pinput = input)
                    Status = (int)dll_import.ippsAddC_32f_I(val, pinput, len);
            }
            public unsafe void add(ref float[] input, int from, float val, int len)
            {
                fixed (float* pinput = input)
                    Status = (int)dll_import.ippsAddC_32f_I(val, pinput + from, len);
            }
            public unsafe void add(ref float[] input, ref float[] output)
            {
                fixed (float* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsAdd_32f_I(pinput, poutput, input.Length);
            }
            public unsafe void add(ref float[] input, int from1, ref float[] output, int from2, int len)
            {
                fixed (float* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsAdd_32f_I(pinput+ from1, poutput+ from2, len);
            }
        }
        public class Conj
        {
            int Status;
            public unsafe void conj(ref ComplexF[] input)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsConj_32fc_I(pinput, input.Length);
            }
            public unsafe void conj(ref ComplexF[] input, int from, int len)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsConj_32fc_I(pinput + from, len);
            }
            public unsafe void conjFlip(ref ComplexF[] input, ref ComplexF[] output)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsConjFlip_32fc(pinput, poutput, input.Length);
            }
            public unsafe void conjFlip(ref ComplexF[] input, int from, int Len, ref ComplexF[] output, int fromo)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsConjFlip_32fc(pinput + from, poutput + fromo, Len);
            }
            public unsafe void conj(short* input, int from, int len)
            {
                Status = (int)dll_import.ippsConj_16sc_I((ComplexS*)(input + from), len);
            }
        }
        public class FilterLow
        {
            public int Status;
            public double RFreq;
            public int Length;
            public IppWinType WinType;
            public bool DoNormal;
            int buffLen;
            byte[] Buffer;
            public bool createAgain = false;
            public Convert convert = new Convert();
            public unsafe int create(double rFreq, ref double[] filter, IppWinType winType, bool doNormal)
            {
                if (Status == 0 && RFreq == rFreq && Length == filter.Length && WinType == winType && DoNormal == doNormal && !createAgain)
                    return Status;
                createAgain = false;
                RFreq = rFreq;
                Length = filter.Length;
                WinType = winType;
                DoNormal = doNormal;
                fixed (int* pbuffLen = &buffLen)
                    Status = (int)dll_import.ippsFIRGenGetBufferSize(filter.Length, pbuffLen);
                if (Buffer == null || Buffer.Length < buffLen)
                    Buffer = new byte[buffLen];
                fixed (double* ptaps = filter)
                fixed (byte* pBuffer = Buffer)
                    Status = (int)dll_import.ippsFIRGenLowpass_64f(rFreq, ptaps, filter.Length, winType, doNormal, pBuffer);
                return Status;
            }
            public unsafe int create(double rFreq, ref double[] filter, int from, int tapsLen, IppWinType winType, bool doNormal)
            {
                if (Status == 0 && RFreq == rFreq && Length == tapsLen && WinType == winType && DoNormal == doNormal && !createAgain)
                    return Status;
                createAgain = false;
                RFreq = rFreq;
                Length = tapsLen;
                WinType = winType;
                DoNormal = doNormal;
                fixed (int* pbuffLen = &buffLen)
                    Status = (int)dll_import.ippsFIRGenGetBufferSize(tapsLen, pbuffLen);
                if (Buffer == null || Buffer.Length < buffLen)
                    Buffer = new byte[buffLen];
                fixed (double* ptaps = filter)
                fixed (byte* pBuffer = Buffer)
                    Status = (int)dll_import.ippsFIRGenLowpass_64f(rFreq, ptaps + from, tapsLen, winType, doNormal, pBuffer);
                return Status;
            }
            public unsafe int create(double rFreq, ref float[] filter, IppWinType winType, bool doNormal)
            {
                if (Status == 0 && RFreq == rFreq && Length == filter.Length && WinType == winType && DoNormal == doNormal && !createAgain)
                    return Status;
                createAgain = false;
                RFreq = rFreq;
                Length = filter.Length;
                WinType = winType;
                DoNormal = doNormal;
                fixed (int* pbuffLen = &buffLen)
                    Status = (int)dll_import.ippsFIRGenGetBufferSize(filter.Length, pbuffLen);
                if (Buffer == null || Buffer.Length < buffLen)
                    Buffer = new byte[buffLen];
                double[] filter_tmp = new double[Length];
                fixed (double* ptaps = filter_tmp)
                fixed (byte* pBuffer = Buffer)
                    Status = (int)dll_import.ippsFIRGenLowpass_64f(rFreq, ptaps, filter.Length, winType, doNormal, pBuffer);
                convert.double_to_float(ref filter_tmp, ref filter);
                return Status;
            }
            public unsafe int create(double rFreq, ref float[] filter, int from, int tapsLen, IppWinType winType, bool doNormal)
            {
                if (Status == 0 && RFreq == rFreq && Length == tapsLen && WinType == winType && DoNormal == doNormal && !createAgain)
                    return Status;
                createAgain = false;
                RFreq = rFreq;
                Length = tapsLen;
                WinType = winType;
                DoNormal = doNormal;
                fixed (int* pbuffLen = &buffLen)
                    Status = (int)dll_import.ippsFIRGenGetBufferSize(tapsLen, pbuffLen);
                if (Buffer == null || Buffer.Length < buffLen)
                    Buffer = new byte[buffLen];
                double[] filter_tmp = new double[Length];
                fixed (double* ptaps = filter_tmp)
                fixed (byte* pBuffer = Buffer)
                    Status = (int)dll_import.ippsFIRGenLowpass_64f(rFreq, ptaps + from, tapsLen, winType, doNormal, pBuffer);
                convert.double_to_float(ref filter_tmp, ref filter);
                return Status;
            }
        }
        public class FilterHigh
        {
            public int Status;
            public double RFreq;
            public int Length;
            public IppWinType WinType;
            public bool DoNormal;
            int buffLen;
            byte[] Buffer;
            public bool createAgain = false;
            public unsafe int create(double rFreq, ref double[] filter, IppWinType winType, bool doNormal)
            {
                if (Status == 0 && RFreq == rFreq && Length == filter.Length && WinType == winType && DoNormal == doNormal && !createAgain)
                    return Status;
                createAgain = false;
                RFreq = rFreq;
                Length = filter.Length;
                WinType = winType;
                DoNormal = doNormal;
                fixed (int* pbuffLen = &buffLen)
                    Status = (int)dll_import.ippsFIRGenGetBufferSize(filter.Length, pbuffLen);
                if (Buffer == null || Buffer.Length < buffLen)
                    Buffer = new byte[buffLen];
                fixed (double* ptaps = filter)
                fixed (byte* pBuffer = Buffer)
                    Status = (int)dll_import.ippsFIRGenHighpass_64f(rFreq, ptaps, filter.Length, winType, doNormal, pBuffer);
                return Status;
            }
            public unsafe int create(double rFreq, ref double[] filter, int from, int tapsLen, IppWinType winType, bool doNormal)
            {
                if (Status == 0 && RFreq == rFreq && Length == tapsLen && WinType == winType && DoNormal == doNormal && !createAgain)
                    return Status;
                createAgain = false;
                RFreq = rFreq;
                Length = tapsLen;
                WinType = winType;
                DoNormal = doNormal;
                fixed (int* pbuffLen = &buffLen)
                    Status = (int)dll_import.ippsFIRGenGetBufferSize(tapsLen, pbuffLen);
                if (Buffer == null || Buffer.Length < buffLen)
                    Buffer = new byte[buffLen];
                fixed (double* ptaps = filter)
                fixed (byte* pBuffer = Buffer)
                    Status = (int)dll_import.ippsFIRGenHighpass_64f(rFreq, ptaps + from, tapsLen, winType, doNormal, pBuffer);
                return Status;
            }
        }
        public class FilterBandPass
        {
            public int Status;
            public double RFreq;
            public int Length;
            public IppWinType WinType;
            public bool DoNormal;
            int buffLen;
            byte[] Buffer;
            public bool createAgain = false;
            Convert convert = new Convert();
            public unsafe int create(double rLowFreq, double rHighFreq, ref double[] filter, IppWinType winType, bool doNormal)
            {
                if (Status == 0 && RFreq == rLowFreq && Length == filter.Length && WinType == winType && DoNormal == doNormal && !createAgain)
                    return Status;
                createAgain = false;
                RFreq = rLowFreq;
                Length = filter.Length;
                WinType = winType;
                DoNormal = doNormal;
                fixed (int* pbuffLen = &buffLen)
                    Status = (int)dll_import.ippsFIRGenGetBufferSize(filter.Length, pbuffLen);
                if (Buffer == null || Buffer.Length < buffLen)
                    Buffer = new byte[buffLen];
                fixed (double* ptaps = filter)
                fixed (byte* pBuffer = Buffer)
                    Status = (int)dll_import.ippsFIRGenBandpass_64f(rLowFreq, rHighFreq, ptaps, filter.Length, winType, doNormal, pBuffer);
                return Status;
            }
            public unsafe int create(double rLowFreq, double rHighFreq, ref double[] filter, int from, int tapsLen, IppWinType winType, bool doNormal)
            {
                if (Status == 0 && RFreq == rLowFreq && Length == tapsLen && WinType == winType && DoNormal == doNormal && !createAgain)
                    return Status;
                createAgain = false;
                RFreq = rLowFreq;
                Length = tapsLen;
                WinType = winType;
                DoNormal = doNormal;
                fixed (int* pbuffLen = &buffLen)
                    Status = (int)dll_import.ippsFIRGenGetBufferSize(tapsLen, pbuffLen);
                if (Buffer == null || Buffer.Length < buffLen)
                    Buffer = new byte[buffLen];
                fixed (double* ptaps = filter)
                fixed (byte* pBuffer = Buffer)
                    Status = (int)dll_import.ippsFIRGenBandpass_64f(rLowFreq, rHighFreq, ptaps + from, tapsLen, winType, doNormal, pBuffer);
                return Status;
            }
            public unsafe int create(double rLowFreq, double rHighFreq, ref float[] filter, IppWinType winType, bool doNormal)
            {
                if (Status == 0 && RFreq == rLowFreq && Length == filter.Length && WinType == winType && DoNormal == doNormal && !createAgain)
                    return Status;
                createAgain = false;
                RFreq = rLowFreq;
                Length = filter.Length;
                WinType = winType;
                DoNormal = doNormal;
                double[] filter_tmp = new double[filter.Length];
                fixed (int* pbuffLen = &buffLen)
                    Status = (int)dll_import.ippsFIRGenGetBufferSize(filter.Length, pbuffLen);
                if (Buffer == null || Buffer.Length < buffLen)
                    Buffer = new byte[buffLen];
                fixed (double* ptaps = filter_tmp)
                fixed (byte* pBuffer = Buffer)
                    Status = (int)dll_import.ippsFIRGenBandpass_64f(rLowFreq, rHighFreq, ptaps, filter.Length, winType, doNormal, pBuffer);
                convert.double_to_float(ref filter_tmp, ref filter);
                return Status;
            }
            public unsafe int create(double rLowFreq, double rHighFreq, ref float[] filter, int from, int tapsLen, IppWinType winType, bool doNormal)
            {
                if (Status == 0 && RFreq == rLowFreq && Length == tapsLen && WinType == winType && DoNormal == doNormal && !createAgain)
                    return Status;
                createAgain = false;
                RFreq = rLowFreq;
                Length = tapsLen;
                WinType = winType;
                DoNormal = doNormal;
                double[] filter_tmp = new double[tapsLen];
                filter = new float[tapsLen];
                fixed (int* pbuffLen = &buffLen)
                    Status = (int)dll_import.ippsFIRGenGetBufferSize(tapsLen, pbuffLen);
                if (Buffer == null || Buffer.Length < buffLen)
                    Buffer = new byte[buffLen];
                fixed (double* ptaps = filter_tmp)
                fixed (byte* pBuffer = Buffer)
                    Status = (int)dll_import.ippsFIRGenBandpass_64f(rLowFreq, rHighFreq, ptaps + from, tapsLen, winType, doNormal, pBuffer);
                convert.double_to_float(ref filter_tmp, ref filter);
                return Status;
            }
        }
        public class FilterBandStop
        {
            public int Status;
            public double RFreq;
            public int Length;
            public IppWinType WinType;
            public bool DoNormal;
            int buffLen;
            byte[] Buffer;
            public bool createAgain = false;
            public unsafe int create(double rLowFreq, double rHighFreq, ref double[] filter, IppWinType winType, bool doNormal)
            {
                if (Status == 0 && RFreq == rLowFreq && Length == filter.Length && WinType == winType && DoNormal == doNormal && !createAgain)
                    return Status;
                createAgain = false;
                RFreq = rLowFreq;
                Length = filter.Length;
                WinType = winType;
                DoNormal = doNormal;
                fixed (int* pbuffLen = &buffLen)
                    Status = (int)dll_import.ippsFIRGenGetBufferSize(filter.Length, pbuffLen);
                if (Buffer == null || Buffer.Length < buffLen)
                    Buffer = new byte[buffLen];
                fixed (double* ptaps = filter)
                fixed (byte* pBuffer = Buffer)
                    Status = (int)dll_import.ippsFIRGenBandstop_64f(rLowFreq, rHighFreq, ptaps, filter.Length, winType, doNormal, pBuffer);
                return Status;
            }
            public unsafe int create(double rLowFreq, double rHighFreq, ref double[] filter, int from, int tapsLen, IppWinType winType, bool doNormal)
            {
                if (Status == 0 && RFreq == rLowFreq && Length == tapsLen && WinType == winType && DoNormal == doNormal && !createAgain)
                    return Status;
                createAgain = false;
                RFreq = rLowFreq;
                Length = tapsLen;
                WinType = winType;
                DoNormal = doNormal;
                fixed (int* pbuffLen = &buffLen)
                    Status = (int)dll_import.ippsFIRGenGetBufferSize(tapsLen, pbuffLen);
                if (Buffer == null || Buffer.Length < buffLen)
                    Buffer = new byte[buffLen];
                fixed (double* ptaps = filter)
                fixed (byte* pBuffer = Buffer)
                    Status = (int)dll_import.ippsFIRGenBandstop_64f(rLowFreq, rHighFreq, ptaps + from, tapsLen, winType, doNormal, pBuffer);
                return Status;
            }
        }
        public class Not
        {
            int Status;
            public unsafe void not(ref byte[] input, int len)
            {
                fixed (byte* pinput = input)
                    Status = (int)dll_import.ippsNot_8u_I(pinput, len);
            }

        }
        public class AND
        {
            int Status;
            public unsafe void and(byte val, ref byte[] input, int len)
            {
                fixed (byte* pinput = input)
                    Status = (int)dll_import.ippsAndC_8u_I(val, pinput, len);
            }
        }
        public class Sub
        {
            int Status;
            public unsafe void sub(byte val, ref byte[] pSrcDst, int len, int scaleFactor)
            {
                fixed (byte* pSrcDst1 = pSrcDst)
                    Status = (int)dll_import.ippsSubC_8u_ISfs(val, pSrcDst1, len, scaleFactor);
            }

        }
        public class Xor
        {
            int Status;
            public unsafe void xor(ref byte[] input1, int from1, ref byte[] input2, int from2, ref byte[] output, int len)
            {
                fixed (byte* pinput1 = input1)
                fixed (byte* pinput2 = input2)
                fixed (byte* poutput = output)
                    Status = (int)dll_import.ippsXor_8u(pinput1 + from1, pinput2 + from2, poutput, len);
            }

        }
        public class Flip
        {
            int Status;
            public unsafe void flip(ref ComplexF[] input)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsFlip_32fc_I(pinput, input.Length);
            }
            public unsafe void flip(ref ComplexF[] input, ref ComplexF[] output)
            {
                fixed (ComplexF* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsFlip_32fc(pinput, poutput, input.Length);
            }
            public unsafe void flip(ref float[] input)
            {
                fixed (float* pinput = input)
                    Status = (int)dll_import.ippsFlip_32f_I(pinput, input.Length);
            }
            public unsafe void flip(ref float[] input, ref float[] output)
            {
                fixed (float* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsFlip_32f(pinput, poutput, input.Length);
            }
        }
        public class Convert_Real_Imag
        {
            int Status;
            public unsafe void real2complex(ref float[] real, ref ComplexF[] output)
            {
                fixed (ComplexF* poutput = output)
                fixed (float* preal = real)
                    Status = (int)dll_import.ippsRealToCplx_32f(preal, null, poutput, real.Length);
            }
            public unsafe void real2complex(ref float[] real, int from_inp, ref ComplexF[] output, int from_out, int len)
            {
                fixed (ComplexF* poutput = output)
                fixed (float* preal = real)
                    Status = (int)dll_import.ippsRealToCplx_32f(preal + from_inp, null, poutput + from_out, len);
            }
            public unsafe void imag2complex(ref float[] imag, ref ComplexF[] output)
            {
                fixed (ComplexF* poutput = output)
                fixed (float* pimag = imag)
                    Status = (int)dll_import.ippsRealToCplx_32f(null, pimag, poutput, imag.Length);
            }
            public unsafe void real_imag_2_complex(ref float[] real, ref float[] imag, ref ComplexF[] output)
            {
                fixed (ComplexF* poutput = output)
                fixed (float* preal = real, pimag = imag)
                    Status = (int)dll_import.ippsRealToCplx_32f(preal, pimag, poutput, real.Length);
            }
            public unsafe void complex2real(ref ComplexF[] output, ref float[] real)
            {
                fixed (ComplexF* poutput = output)
                fixed (float* preal = real)
                    Status = (int)dll_import.ippsReal_32fc(poutput, preal, real.Length);
            }
            public unsafe void complex2real(ref ComplexF[] output, int from_out, int len, ref float[] real, int from_inp)
            {
                fixed (ComplexF* poutput = output)
                fixed (float* preal = real)
                    Status = (int)dll_import.ippsReal_32fc(poutput + from_out, preal + from_inp, len);
            }
            public unsafe void complex2imag(ref ComplexF[] output, ref float[] imag)
            {
                fixed (ComplexF* poutput = output)
                fixed (float* preal = imag)
                    Status = (int)dll_import.ippsImag_32fc(poutput, preal, imag.Length);
            }
            public unsafe void complex2imag(ref ComplexF[] output, int from_out, int len, ref float[] imag, int from_inp)
            {
                fixed (ComplexF* poutput = output)
                fixed (float* preal = imag)
                    Status = (int)dll_import.ippsImag_32fc(poutput + from_out, preal + from_inp, len);
            }
        }
        public class Pow_to_x
        {
            int Status;
            public unsafe void Pow_x(ref ComplexF[] input, ComplexF pow)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsPowx_32fc_A21(pinput, pow, pinput, input.Length);
            }
            public unsafe void Pow_x(ref ComplexF[] input, ComplexF pow, int from, int len)
            {
                fixed (ComplexF* pinput = input)
                    Status = (int)dll_import.ippsPowx_32fc_A21(pinput + from, pow, pinput + from, len);
            }
            public unsafe void Pow_x(ref ComplexF[] input, ref ComplexF[] output, ComplexF pow)
            {
                fixed (ComplexF* pinput = input)
                fixed (ComplexF* poutput = output)
                    Status = (int)dll_import.ippsPowx_32fc_A21(pinput, pow, poutput, input.Length);
            }
            public unsafe void Pow_x(ref ComplexF[] input, int from, int len, ref ComplexF[] output, int from_out, ComplexF pow)
            {
                fixed (ComplexF* pinput = input) fixed (ComplexF* poutput = output)
                    Status = (int)dll_import.ippsPowx_32fc_A21(pinput + from, pow, poutput + from_out, len);
            }
        }
        public class Log10
        {
            int Status;
            public unsafe void log10(ref float[] input, ref float[] output)
            {
                fixed (float* pinput = input, poutput = output)
                    Status = (int)dll_import.ippsLog10_32f_A11(pinput, poutput, input.Length);
            }
            public unsafe void log10(ref float[] input, ref ComplexF[] output)
            {
                fixed (float* pinput = input)
                fixed (ComplexF* poutput = output)
                    Status = (int)dll_import.ippsLog10_32f_A11(pinput, (float*)poutput, input.Length);
            }
            public unsafe void log10(ref float[] input)
            {
                fixed (float* pinput = input)
                    Status = (int)dll_import.ippsLog10_32f_A11(pinput, pinput, input.Length);
            }
        }
        public class Min_Max
        {
            int Status;
            public unsafe void Max(ref float[] input, ref float max_value)
            {
                fixed (float* pinput = input, pmax_value = &max_value)
                    Status = (int)dll_import.ippsMax_32f(pinput, input.Length, pmax_value);
            }
            public unsafe float Max(ref float[] input)
            {
                float tmp = -1;
                fixed (float* pinput = input)
                    Status = (int)dll_import.ippsMax_32f(pinput, input.Length, &tmp);
                return tmp;
            }
            public unsafe void Min(ref float[] input, ref float min_value)
            {
                fixed (float* pinput = input, pmin_value = &min_value)
                    Status = (int)dll_import.ippsMin_32f(pinput, input.Length, pmin_value);
            }
            public unsafe float Min(ref float[] input)
            {
                float tmp = -1;
                fixed (float* pinput = input)
                    Status = (int)dll_import.ippsMin_32f(pinput, input.Length, &tmp);
                return tmp;
            }
            public unsafe void Max_Index(ref float[] input, ref float max_value, ref int max_index)
            {
                fixed (float* pinput = input)
                    Status = (int)dll_import.ippsMaxIndx_32f(pinput, input.Length, ref max_value, ref max_index);
            }
            public unsafe void Min_Index(ref float[] input, ref float min_value, ref int min_index)
            {
                fixed (float* pinput = input)
                    Status = (int)dll_import.ippsMaxIndx_32f(pinput, input.Length, ref min_value, ref min_index);
            }
        }
        public class Convert
        {
            int Status;
            public unsafe void float_to_short(ref float[] input, ref short[] output)
            {
                fixed (float* pinput = input)
                fixed (short* poutput = output)
                    Status = (int)dll_import.ippsConvert_32f16s_Sfs(pinput, poutput, input.Length
                        , IppRoundMode.ippRndZero, 0);
            }
            public unsafe void float_to_short(ref ComplexF[] input, ref byte[] output)
            {
                fixed (ComplexF* pinput = input)
                fixed (byte* poutput = output)
                    Status = (int)dll_import.ippsConvert_32f16s_Sfs((float*)pinput, (short*)poutput, input.Length * 2
                        , IppRoundMode.ippRndZero, 0);
            }
            public unsafe void float_to_short(ref float[] input, ref short[] output, int scale_factor)
            {
                fixed (float* pinput = input)
                fixed (short* poutput = output)
                    Status = (int)dll_import.ippsConvert_32f16s_Sfs(pinput, poutput, input.Length
                        , IppRoundMode.ippRndZero, scale_factor);
            }
            public unsafe void float_to_short(ref ComplexF[] input, ref byte[] output, int scale_factor)
            {
                fixed (ComplexF* pinput = input)
                fixed (byte* poutput = output)
                    Status = (int)dll_import.ippsConvert_32f16s_Sfs((float*)pinput, (short*)poutput, input.Length * 2
                        , IppRoundMode.ippRndZero, scale_factor);
            }
            public unsafe void ushort_to_float(ref short[] input, ref float[] output)
            {
                fixed (short* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsConvert_16u32f(pinput, poutput, input.Length);
            }
            public unsafe void ushort_to_float(ref short[] input, int from_in, int len, ref float[] output, int from_ou)
            {
                fixed (short* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsConvert_16u32f(pinput + from_in, poutput + from_ou, len);
            }
            public unsafe void ushort_to_float(short* input, int from_in, int len, ref float[] output, int from_ou)
            {
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsConvert_16u32f(input + from_in, poutput + from_ou, len);
            }
            public unsafe void ushort_to_float(short* input, int from_in, int len, ref ComplexF[] output, int from_ou)
            {
                fixed (ComplexF* poutput = output)
                    Status = (int)dll_import.ippsConvert_16u32f(input + from_in, (float*)(poutput + from_ou), len);
            }
            public unsafe void short_to_float(ref short[] input, ref float[] output)
            {
                fixed (short* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsConvert_16s32f(pinput, poutput, input.Length);
            }
            public unsafe void short_to_float(ref short[] input, int from_in, int len, ref float[] output, int from_ou)
            {
                fixed (short* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsConvert_16s32f(pinput + from_in, poutput + from_ou, len);
            }
            public unsafe void short_to_float(short* input, int from_in, int len, ref float[] output, int from_ou)
            {
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsConvert_16s32f(input + from_in, poutput + from_ou, len);
            }
            public unsafe void short_to_float(short* input, int from_in, int len, ref ComplexF[] output, int from_ou)
            {
                fixed (ComplexF* poutput = output)
                    Status = (int)dll_import.ippsConvert_16s32f(input + from_in, (float*)(poutput + from_ou), len);
            }
            public unsafe void sbyte_to_float(ref byte[] input, ref float[] output)
            {
                fixed (byte* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsConvert_8s32f(pinput, poutput, input.Length);
            }
            public unsafe void sbyte_to_float(ref byte[] input, int from_in, int len, ref float[] output, int from_ou)
            {
                fixed (byte* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsConvert_8s32f(pinput + from_in, poutput + from_ou, len);
            }
            public unsafe void sbyte_to_float(byte* input, int from_in, int len, ref ComplexF[] output, int from_ou)
            {
                fixed (ComplexF* poutput = output)
                    Status = (int)dll_import.ippsConvert_8s32f(input + from_in, (float*)(poutput + from_ou), len);
            }
            public unsafe void ubyte_to_float(ref byte[] input, ref float[] output)
            {
                fixed (byte* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsConvert_8u32f(pinput, poutput, input.Length);
            }
            public unsafe void ubyte_to_float(ref byte[] input, int from_in, int len, ref float[] output, int from_ou)
            {
                fixed (byte* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsConvert_8u32f(pinput + from_in, poutput + from_ou, len);
            }
            public unsafe void ubyte_to_float(byte* input, int from_in, int len, ref ComplexF[] output, int from_ou)
            {
                fixed (ComplexF* poutput = output)
                    Status = (int)dll_import.ippsConvert_8u32f(input + from_in, (float*)(poutput + from_ou), len);
            }
            public unsafe void ubyte_to_short(byte* input, int from_in, int len, ref byte[] output, int from_ou)
            {
                fixed (byte* poutput = output)
                    Status = (int)dll_import.ippsConvert_8s16s(input + from_in, (short*)(poutput) + from_ou, len);

            }
            public unsafe void double_to_float(ref double[] input, ref float[] output)
            {
                fixed (double* pinput = input)
                fixed (float* poutput = output)
                    Status = (int)dll_import.ippsConvert_64f32f(pinput, poutput, input.Length);
            }
        }
        public class Swap
        {
            int Status;
            public unsafe void int16(ref short[] input)
            {
                fixed (short* pinput = input)
                    Status = (int)dll_import.ippsSwapBytes_16u_I(pinput, input.Length);
            }
            public unsafe void int16(ref byte[] input)
            {
                fixed (byte* pinput = input)
                    Status = (int)dll_import.ippsSwapBytes_16u_I((short*)pinput, input.Length);
            }
        }
        public class LZSS
        {
            private IppStatus Status;
            private byte[] State;
            private int state_length;
            private unsafe byte* rrv1, wecf;
            public LZSS()
            {
                unsafe
                {
                    fixed (int* ptr = &state_length)
                        Status = dll_import.ippsLZSSGetSize_8u(ptr);
                    if (Status == IppStatus.ippStsNoErr)
                    {
                        State = new byte[state_length];
                        fixed (byte* pState = State)
                            Status = dll_import.ippsEncodeLZSSInit_8u(pState);
                    }
                }
            }
            public int Encode(ref byte[] Src, ref int SrcLen, ref byte[] Dst, ref int DstLen)
            {
                if (Status != IppStatus.ippStsNoErr)
                    return (int)Status;
                unsafe
                {
                    fixed (byte* pSrc = Src, pDst = Dst, pState = State)
                    fixed (int* pDstLen = &DstLen, pSrcLen = &SrcLen)
                    {
                        rrv1 = pSrc;
                        wecf = pDst;
                        fixed (byte** prrv1 = &rrv1, pwecf = &wecf)
                        {
                            Status = dll_import.ippsEncodeLZSS_8u(prrv1, pSrcLen, pwecf, pDstLen, pState);
                        }
                    }
                }
                return (int)Status;
            }
        }


        public class Noise_reduction
        {
            private IppStatus Status;
            private IppsNRLevel Noise_level = IppsNRLevel.ippsNrNormal;
            private int SAMPLING_RATE;
            private int state_size = -1;
            private unsafe float* state;
            public IppsNrMode mode = IppsNrMode.ippsNrUpdateAll;

            ~ Noise_reduction()
            {
                unsafe
                {
                    dll_import.ippsFree(state);
                    state = null;
                }
            }
            /// <summary>
            /// Valid sampling rate: 8000, 16000, 22050, 32000
            /// </summary>
            public unsafe Noise_reduction(double sampling_rate)
            {
                foreach (var item in Enum.GetValues(typeof(IppPCMFrequency)))
                {
                    if ((int)item >= sampling_rate)
                    {
                        SAMPLING_RATE = (int)item;
                        break;
                    }
                }

                fixed (int* pstate_size = &state_size)
                    Status = dll_import.legacy90ippsFilterNoiseGetStateSize_RTA_32f(SAMPLING_RATE, pstate_size);
                if (Status != 0)
                    return;
                state = dll_import.ippsMalloc_32f(state_size);
                unsafe
                {
                    Status = dll_import.legacy90ippsFilterNoiseInit_RTA_32f(SAMPLING_RATE, state);
                    Status = dll_import.legacy90ippsFilterNoiseLevel_RTA_32f(Noise_level, state);
                }
            }
            /// <summary>
            /// Perform noise reduction on a signal.
            /// signal length must be 160
            /// </summary>
            public void do_noise_reduction(ref float[] Signal)
            {
                unsafe
                {
                    fixed (float* pSingnal = Signal)
                    {
                        Status = dll_import.legacy90ippsFilterNoise_RTA_32f_I(pSingnal, state);
                    }
                }
            }
        }
        public class intel_VAD
        {
            private IppStatus Status;
            int state_size = -1;
            short[] state;
            short pToneFlag, pVadFlag;

            /// <summary>
            /// Valid sampling rate: 8000, 16000, 32000.
            /// </summary>
            public intel_VAD(double sampling_rate)
            {
                unsafe
                {
                    fixed (int* pstate_size = &state_size)
                    {
                        Status = dll_import.legacy90ippsVADGetSize_AMRWB_16s(pstate_size);
                    }
                }
                if (Status == 0)
                {
                    state = new short[state_size];
                    unsafe
                    {
                        fixed (short* pstate = state)
                        {
                            Status = dll_import.legacy90ippsVADInit_AMRWB_16s(pstate);
                        }
                    }
                }

            }
            /// <summary>
            /// Calc DFT.
            /// (signal length must be 320)
            /// </summary>
            public void do_vad(ref short[] input)
            {
                unsafe
                {
                    fixed (short* pinput = input, pstate = state, ppToneFlag = &pToneFlag, ppVadFlag = &pVadFlag)
                    {
                        Status = dll_import.legacy90ippsVAD_AMRWB_16s(pinput, pstate, ppToneFlag, ppVadFlag);
                    }
                }
            }
        }
    }
}
