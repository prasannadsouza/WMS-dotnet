export type GlobalLocaleStrings = {
    en:  GlobalStrings;
    se: GlobalStrings;
}

export type ValidationLocaleStrings = {
    en: ValidationStrings;
    se: ValidationStrings;
}

export type MessageLocaleStrings = {
    en: MessageStrings;
    se: MessageStrings;
}

export type Language = {
    id?: number;
    code: string
    name: string;
}

export type GlobalStrings = {
    home: string;
    settings: string;
    login: string;
    logout: string;
    language: string;
    username: string;
    password: string;
    forgotPassword: string;
    sendPasswordResetLink: string;
    cancel: string;
    email: string;
    yes: string;
    no: string;
    confirmTitle: string;
    confirmMessage: string;
    ok: string;
    message: string;
    error: string;
    close: string;
} 

export type ValidationStrings = {
    usernameCannotBeBlank: string;
    passwordCannotBeBlank: string;
    usernameOrPasswordIsInvalid: string;
    emailCannotBeBlank: string;
}

export type MessageStrings = {
    resetEmailLinkSent: string;
}



