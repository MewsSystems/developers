abstract class Singleton {
    
    // -------------------------
    // -- Singleton structure --
    // -------------------------

    private static _instance: Singleton;
    static get instance(): Singleton {
        if (!this._instance) {
            var me: any = this; // any just for typescript
            this._instance = new me();
        }

        return this._instance;
    }
}

export = Singleton