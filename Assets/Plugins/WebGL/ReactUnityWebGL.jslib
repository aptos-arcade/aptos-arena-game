mergeInto(LibraryManager.library, {
      OnTransactionRequest: function (func, args, typeArgs) {
            try {
                window.dispatchReactUnityEvent(
                    "OnTransactionRequest",
                    UTF8ToString(func),
                    UTF8ToString(args),
                    UTF8ToString(typeArgs));
            } catch (e) {
                console.warn("Failed to dispatch event");
            }
      },
      SetConnectModalOpen: function (isOpen) {
            try {
                window.dispatchReactUnityEvent(
                    "SetConnectModalOpen",
                    isOpen
                );
            } catch (e) {
                console.warn("Failed to dispatch event");
            }
      },
});