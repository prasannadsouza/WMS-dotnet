
export type LanguageCulture = {
    id?: number;
    code: string
    name: string;
}

export type GeneralLocaleString = {
    sv_SE: GeneralString;
    en_SE: GeneralString;
}

export type GeneralString = {
    all: string;
    cancel: string;
    close: string;
    confirmMessage: string;
    confirmTitle: string;
    email: string;
    error: string;
    fetchData: string;
    home: string;
    language: string;
    message: string;
    no: string;
    ok: string;
    settings: string;
    to: string;
    yes: string;
    logout: string;
}

export type LoginLocaleString = {
    sv_SE: LoginString;
    en_SE: LoginString;
}

export type LoginString = {
    emailCannotBeBlank: string;
    forgotPassword: string;
    login: string;
    loginTitle: string;
    logout: string;
    password: string;
    passwordCannotBeBlank: string;
    resetEmailLinkSent: string;
    sendPasswordResetLink: string;
    username: string;
    usernameCannotBeBlank: string;
    usernameOrPasswordIsInvalid: string;
}




