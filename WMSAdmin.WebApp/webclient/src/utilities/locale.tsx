import { LanguageCulture, GeneralString, GeneralLocaleString, LoginString, LoginLocaleString } from "../entities/locales"
import { ResponseData } from "../entities/entities";
import { CacheConstants, CultureCodeConstants } from "../entities/constants";
import { Utility } from "./utility";
import { URL } from "url";
export class Locale {

    getLanguages = async () => {
        const data: LanguageCulture[] = Utility.getFromLocalStorage(CacheConstants.LANGUAGECULTURELIST);
        if (data !== undefined && data !== null) {
            let response: ResponseData<LanguageCulture[]> = {
                data: data,
                errors: []
            }
            return Promise.resolve(response);
        };

        return await Utility.GetData<LanguageCulture[]>(new URL("api/Language/GetUICulture"), { data: null }).then(response => {
            if (response.errors?.length > 0 == true) return response;

            let languageCodes: string[];
            languageCodes.push(CultureCodeConstants.en_SE);
            languageCodes.push(CultureCodeConstants.sv_SE);

            const data = response.data.filter(x => languageCodes.includes(x.code));
            
            Utility.saveToLocalStorage(CacheConstants.LANGUAGECULTURELIST, data);
            return response;
        });
    };

    getGeneralString = async () => {
        let response: ResponseData<GeneralLocaleString> = {};
        const languagesResponse = await this.getLanguages();

        if (languagesResponse.errors?.length > 0 == true) {
            response.errors = languagesResponse.errors;
            return response;
        }
        
        for (var i = 0; i < languagesResponse.data.length; i++) {
            const item = languagesResponse.data[i];
            let url = new URL("api/Language/GetGeneralString");
            url.search = new URLSearchParams({ cultureCode: item.code }).toString();
            await Utility.GetData<GeneralString>(url, { data: null }).then(x => {
                if (x.errors?.length > 0 == true) {
                    response.errors.push(...x.errors);
                    return x;
                }
                
                Utility.saveToLocalStorage(CacheConstants.LANGUAGECULTURELIST + "_" + item.code, response.data);
                return response;
            });     
        }
        
        response.data = {
            en_SE: Utility.getFromLocalStorage(CacheConstants.LANGUAGECULTURELIST + "_" + CultureCodeConstants.en_SE),
            sv_SE: Utility.getFromLocalStorage(CacheConstants.LANGUAGECULTURELIST + "_" + CultureCodeConstants.sv_SE),
        };

        return Promise.resolve(response);;
    };

    getLoginString = async () => {
        let response: ResponseData<LoginLocaleString> = {};
        const languagesResponse = await this.getLanguages();

        if (languagesResponse.errors?.length > 0 == true) {
            response.errors = languagesResponse.errors;
            return response;
        }

        for (var i = 0; i < languagesResponse.data.length; i++) {
            const item = languagesResponse.data[i];
            let url = new URL("api/Language/GetLoginString");
            url.search = new URLSearchParams({ cultureCode: item.code }).toString();
            await Utility.GetData<LoginString>(url, { data: null }).then(x => {
                if (x.errors?.length > 0 == true) {
                    response.errors = x.errors;
                    return x;
                }

                Utility.saveToLocalStorage(CacheConstants.LANGUAGECULTURELIST + "_" + item.code, response.data);
                return response;
            });
        }

        response.data = {
            en_SE: Utility.getFromLocalStorage(CacheConstants.LANGUAGECULTURELIST + "_" + CultureCodeConstants.en_SE),
            sv_SE: Utility.getFromLocalStorage(CacheConstants.LANGUAGECULTURELIST + "_" + CultureCodeConstants.sv_SE),
        };

        return Promise.resolve(response);;
    };
}