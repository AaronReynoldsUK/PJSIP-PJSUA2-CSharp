//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.12
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


public class MediaFmtChangedEvent : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal MediaFmtChangedEvent(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(MediaFmtChangedEvent obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~MediaFmtChangedEvent() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          pjsua2PINVOKE.delete_MediaFmtChangedEvent(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public uint newWidth {
    set {
      pjsua2PINVOKE.MediaFmtChangedEvent_newWidth_set(swigCPtr, value);
    } 
    get {
      uint ret = pjsua2PINVOKE.MediaFmtChangedEvent_newWidth_get(swigCPtr);
      return ret;
    } 
  }

  public uint newHeight {
    set {
      pjsua2PINVOKE.MediaFmtChangedEvent_newHeight_set(swigCPtr, value);
    } 
    get {
      uint ret = pjsua2PINVOKE.MediaFmtChangedEvent_newHeight_get(swigCPtr);
      return ret;
    } 
  }

  public MediaFmtChangedEvent() : this(pjsua2PINVOKE.new_MediaFmtChangedEvent(), true) {
  }

}
