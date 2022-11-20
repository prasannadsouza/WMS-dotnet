import { LanguageCulture, GeneralString, GeneralLocaleString, LoginLocaleString } from "../entities/locales"
import { ResponseData } from "../entities/entities";
import { APIParts, CacheConstants, CultureCodeConstants } from "../entities/constants";
import { Utility } from "./utility";

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

        return await Utility.GetData<LanguageCulture[]>(APIParts.LANGUAGE + "GetUICulture", undefined, { data: null }).then(response => {
            if ((response.errors?.length > 0) === true) return response;

            let languageCodes: string[] = [];
            languageCodes.push(CultureCodeConstants.en_SE);
            languageCodes.push(CultureCodeConstants.sv_SE);

            const data = response.data.filter(x => languageCodes.includes(x.code));
            
            Utility.saveToLocalStorage(CacheConstants.LANGUAGECULTURELIST, data);
            return response;
        });
    };

    getGeneralString = async () => {
        let responseData: ResponseData<GeneralLocaleString> = {};
        const languagesResponse = await this.getLanguages();

        if ((languagesResponse.errors?.length > 0) === true) {
            responseData.errors = languagesResponse.errors;
            return responseData;
        }
        
        for (var i = 0; i < languagesResponse.data.length; i++) {
            const item = languagesResponse.data[i];

            const key = CacheConstants.LANGUAGESTRINGGENERAL + "_" + item.code;
            const data: GeneralString = Utility.getFromLocalStorage(key);

            if (data !== undefined) continue;
            await Utility.GetData<GeneralString>(APIParts.LANGUAGE + "GetGeneralString", new URLSearchParams({ cultureCode: item.code }), { data: null }).then(response => {
                if ((response.errors?.length > 0) === true) {
                    responseData.errors.push(...response.errors);
                    return responseData;
                }

                Utility.saveToLocalStorage(key, response.data);

            });
        }
        
        responseData.data = {
            en_SE: Utility.getFromLocalStorage(CacheConstants.LANGUAGESTRINGGENERAL + "_" + CultureCodeConstants.en_SE),
            sv_SE: Utility.getFromLocalStorage(CacheConstants.LANGUAGESTRINGGENERAL + "_" + CultureCodeConstants.sv_SE),
        };

        return await Promise.resolve(responseData);;
    };

    getLoginString = async () => {
        let responseData: ResponseData<LoginLocaleString> = {};
        const languagesResponse = await this.getLanguages();

        if ((languagesResponse.errors?.length > 0) === true) {
            responseData.errors = languagesResponse.errors;
            return responseData;
        }

        for (var i = 0; i < languagesResponse.data.length; i++) {
            const item = languagesResponse.data[i];

            var key = CacheConstants.LANGUAGESTRINGLOGIN + "_" + item.code;
            const data: GeneralString = Utility.getFromLocalStorage(key);

            if (data !== undefined) continue;

            await Utility.GetData<GeneralString>(APIParts.LANGUAGE + "GetLoginString", new URLSearchParams({ cultureCode: item.code }), { data: null }).then(response => {
                if ((response.errors?.length > 0) === true) {
                    responseData.errors = response.errors;
                    return responseData;
                }

                Utility.saveToLocalStorage(CacheConstants.LANGUAGESTRINGLOGIN + "_" + item.code, response.data);
            });
        }

        responseData.data = {
            en_SE: Utility.getFromLocalStorage(CacheConstants.LANGUAGESTRINGLOGIN + "_" + CultureCodeConstants.en_SE),
            sv_SE: Utility.getFromLocalStorage(CacheConstants.LANGUAGESTRINGLOGIN + "_" + CultureCodeConstants.sv_SE),
        };

        return await Promise.resolve(responseData);;
    };
}