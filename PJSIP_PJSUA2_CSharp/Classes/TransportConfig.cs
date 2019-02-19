//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class TransportConfig : PersistentObject {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal TransportConfig(global::System.IntPtr cPtr, bool cMemoryOwn) : base(pjsua2PINVOKE.TransportConfig_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(TransportConfig obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~TransportConfig() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          pjsua2PINVOKE.delete_TransportConfig(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  public uint port {
    set {
      pjsua2PINVOKE.TransportConfig_port_set(swigCPtr, value);
    } 
    get {
      uint ret = pjsua2PINVOKE.TransportConfig_port_get(swigCPtr);
      return ret;
    } 
  }

  public uint portRange {
    set {
      pjsua2PINVOKE.TransportConfig_portRange_set(swigCPtr, value);
    } 
    get {
      uint ret = pjsua2PINVOKE.TransportConfig_portRange_get(swigCPtr);
      return ret;
    } 
  }

  public string publicAddress {
    set {
      pjsua2PINVOKE.TransportConfig_publicAddress_set(swigCPtr, value);
      if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      string ret = pjsua2PINVOKE.TransportConfig_publicAddress_get(swigCPtr);
      if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public string boundAddress {
    set {
      pjsua2PINVOKE.TransportConfig_boundAddress_set(swigCPtr, value);
      if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
    } 
    get {
      string ret = pjsua2PINVOKE.TransportConfig_boundAddress_get(swigCPtr);
      if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
      return ret;
    } 
  }

  public TlsConfig tlsConfig {
    set {
      pjsua2PINVOKE.TransportConfig_tlsConfig_set(swigCPtr, TlsConfig.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = pjsua2PINVOKE.TransportConfig_tlsConfig_get(swigCPtr);
      TlsConfig ret = (cPtr == global::System.IntPtr.Zero) ? null : new TlsConfig(cPtr, false);
      return ret;
    } 
  }

  public pj_qos_type qosType {
    set {
      pjsua2PINVOKE.TransportConfig_qosType_set(swigCPtr, (int)value);
    } 
    get {
      pj_qos_type ret = (pj_qos_type)pjsua2PINVOKE.TransportConfig_qosType_get(swigCPtr);
      return ret;
    } 
  }

  public pj_qos_params qosParams {
    set {
      pjsua2PINVOKE.TransportConfig_qosParams_set(swigCPtr, pj_qos_params.getCPtr(value));
    } 
    get {
      global::System.IntPtr cPtr = pjsua2PINVOKE.TransportConfig_qosParams_get(swigCPtr);
      pj_qos_params ret = (cPtr == global::System.IntPtr.Zero) ? null : new pj_qos_params(cPtr, false);
      return ret;
    } 
  }

  public TransportConfig() : this(pjsua2PINVOKE.new_TransportConfig(), true) {
  }

  public override void readObject(ContainerNode node) {
    pjsua2PINVOKE.TransportConfig_readObject(swigCPtr, ContainerNode.getCPtr(node));
    if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
  }

  public override void writeObject(ContainerNode node) {
    pjsua2PINVOKE.TransportConfig_writeObject(swigCPtr, ContainerNode.getCPtr(node));
    if (pjsua2PINVOKE.SWIGPendingException.Pending) throw pjsua2PINVOKE.SWIGPendingException.Retrieve();
  }

}
