//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class RtcpFbConfig : PersistentObject {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal RtcpFbConfig(global::System.IntPtr cPtr, bool cMemoryOwn) : base(pjsua2PINVOKE.RtcpFbConfig_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(RtcpFbConfig obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~RtcpFbConfig() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          pjsua2PINVOKE.delete_RtcpFbConfig(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public bool dontUseAvpf {
    set {
      pjsua2PINVOKE.RtcpFbConfig_dontUseAvpf_set(swigCPtr, value);
    } 
    get {
      bool ret = pjsua2PINVOKE.RtcpFbConfig_dontUseAvpf_get(swigCPtr);
      return ret;
    } 
  }

  public SWIGTYPE_p_std__vectorT_pj__RtcpFbCap_t caps {
    set {
      pjsua2PINVOKE.RtcpFbConfig_caps_set(swigCPtr, SWIGTYPE_p_std__vectorT_pj__RtcpFbCap_t.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = pjsua2PINVOKE.RtcpFbConfig_caps_get(swigCPtr);
      SWIGTYPE_p_std__vectorT_pj__RtcpFbCap_t ret = (cPtr == global::System.IntPtr.Zero) ? null : new SWIGTYPE_p_std__vectorT_pj__RtcpFbCap_t(cPtr, false);
      return ret;
    } 
  }

  public RtcpFbConfig() : this(pjsua2PINVOKE.new_RtcpFbConfig(), true) {
  }

  public override void readObject(ContainerNode node) {
    pjsua2PINVOKE.RtcpFbConfig_readObject(swigCPtr, ContainerNode.getCPtr(node));
    if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
  }

  public override void writeObject(ContainerNode node) {
    pjsua2PINVOKE.RtcpFbConfig_writeObject(swigCPtr, ContainerNode.getCPtr(node));
    if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
  }

}
