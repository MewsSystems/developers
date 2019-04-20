class MenuState {
    //public _activeTab: string = "";

    public classNamesRates = "tab";
    public classNamesSettings = "tab";

    public set activeTab(value: string) {
        if (value == "rates") {
            this.classNamesRates = "tab selected";
            this.classNamesSettings = "tab";
        } else {
            this.classNamesRates = "tab";
            this.classNamesSettings = "tab selected";
        }
    }
/*
    public get classNamesRates() {
        var response = "tab";
        if (this.activeTab == "rates") {
            response += " selected";
        }
        return response;
    }

    public get classNamesSettings() {
        var response = "tab";
        if (this.activeTab == "settings") {
            response += " selected";
        }
        return response;
    }*/
}

export = MenuState;