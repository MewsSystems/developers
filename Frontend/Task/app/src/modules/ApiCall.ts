import ERequestMethod from "../abstracts/ERequestMethod";

class ApiCall {
    private _requestMethod: ERequestMethod = ERequestMethod.GET;
    private _endpointUrl: string = "";
    private _method: string = "";
    private _params: { [key: string] : any } = {};

    public set endpoint(value: string) {
        if (value.substr(-1) == "/") {
            value = value.substr(-1);
        }
        this._endpointUrl = value;
    }

    public set method(value: string) {
        this._method = value;
    }

    public addParam(key: string, value: any) {
        this._params[key] = value;
    }

    public requestMethod(value: ERequestMethod) {
        this._requestMethod = value
    }

    public async sendRequest<T>(): Promise<T> {
        var me = this;
        let promise = new Promise<T>(function(resolve, reject) {
            let query = "";
            let requestParams: any = {
                method: me._requestMethod,
                cache: 'no-cache',
                headers: {
                    'Accept': 'application/json'
                }
            }
            if (Object.keys(me._params).length > 0) {
                if (me._requestMethod !== ERequestMethod.GET) {
                    requestParams["body"] = JSON.stringify(me._params);
                } else {
                    // width GET we have to translate object to uri
                    for(var item in me._params) {
                        if (Array.isArray(me._params[item])) {
                            query += "&" + item + "[]=" + me._params[item].join("&" + item + "[]=");
                        } else {
                            query += "&" + item + "=" + me._params[item];
                        }
                    }
                    query = query.substr(1);
                }
            }

            fetch(me._endpointUrl + "/" + me._method + (query !== "" ? "?" + query : ""), requestParams).then(function(response: Response) {
                response.json().then(function(result: T) {
                    resolve(result);
                }).catch(function(err) {
                    reject(err);
                });
            }).catch(function(err) {
                reject(err);
            });
        });

        let res = await promise;
        return res;
    }
}

export = ApiCall;
