export const LinkConstants = {
    HOME: "HOME",
    SETTINGS: "SETTINGS",
    LOGIN: "LOGIN",
    FETCHDATA: "FETCHDATA",
    LOGOUT: "LOGOUT",
};

export const ClientErrorConstants = {
    FETCH_GET: 10001,
    USERNAME_CANNOTBE_BLANK: 10101,
    PASSWORD_CANNOTBE_BLANK: 10102,
    USERNAME_OR_PASSWORD_ISINVALID: 10103,
    EMAIL_CANNOTBE_BLANK: 10104,
    FETCH_POST: 10005,
    

}

export const ServerErrorConstants = {
    AUTHREVALIDATIONREQUIRED: 200005,
}

export const AppConstants = {
    KEY: "MYREACTTSAPP",
}

export const CacheConstants = {
    APPLICATIONCONFIG: "APPLICATIONCONFIG",
    PAGINATIONCONFIG: "PAGINATIONCONFIG",
    LOGINLOCALESTRING: "LOGINLOCALESTRING",
    GENERALOCALESTRING: "GENERALOCALESTRING",
    LANGUAGECULTURELIST: "LANGUAGECULTURELIST",
    LANGUAGESTRINGGENERAL: "LANGUAGESTRINGGENERAL",
    LANGUAGESTRINGLOGIN: "LANGUAGESTRINGLOGIN",
    CONFIGTIMESTAMPS: "CONFIGTIMESTAMPS",
}


export const AppReducerConstants = {
    APP: "APP",
    SET_CURRENTTITLE: "SET_CURRENTTITLE",
    SET_MODEL: "SET_MODEL"
}

export const LocaleCodeConstants = {
    en_SE: "en-SE",
    sv_SE: "sv-SE",
}

export const APIParts = {
    CONFIG: "api/Config/",
    LANGUAGE: "api/Language/",
    APP: "api/App/",
    TEST: "weatherforecast/"
}
