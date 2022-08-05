import { Language, GlobalStrings, GlobalLocaleStrings, ValidationStrings, ValidationLocaleStrings, MessageStrings, MessageLocaleStrings } from "../entities/locales"

export class Locale {

    getLanguages = () => {
        const languages: Language[] = [
            { id: 1, code: "en", name: "English" },
            { id: 2, code: "se", name: "Svenska" }
        ];
        return languages;
    };

    getGlobalStrings = () => {
        const en: GlobalStrings = {
            home: "Home",
            settings: "Settings",
            login: "Login",
            logout: "Logout",
            language: "Language",
            username: "Username",
            password: "Password",
            forgotPassword: "Forgot Password",
            sendPasswordResetLink: "Send Reset Link",
            cancel: "Cancel",
            email: "Email",
            yes: "Yes",
            no: "No",
            confirmTitle: "Please confirm",
            confirmMessage: "Are you sure?",
            ok: "Ok",
            message: "Message",
            error: "Error",
            close: "Close",
        };

        const se: GlobalStrings = {
            home: "Hem",
            settings: "Installingar",
            login: "Logga in",
            logout: "Logga ut",
            language: "Språk",
            username: "Användarnamn",
            password: "Lösenord",
            forgotPassword: "Glömt lösenord",
            sendPasswordResetLink: "Skicka återställningslänk",
            cancel: "Avbryt",
            email: "E-post",
            yes: "Ja",
            no: "Nej",
            confirmTitle: "Vänligen bekräfta",
            confirmMessage: "Är du säker?",
            ok: "Ok",
            message: "Meddelande",
            error: "Fel",
            close: "Stänga",
        };
        const languageLocale: GlobalLocaleStrings = {
            en: en,
            se: se,
        };
        return languageLocale;
    };

    getValidationStrings = () => {
        const en: ValidationStrings = {
            usernameCannotBeBlank: "Username is required",
            passwordCannotBeBlank: "Password is required",
            usernameOrPasswordIsInvalid: "Username or password is invalid",
            emailCannotBeBlank: "Email is required",
        };

        const se: ValidationStrings = {
            usernameCannotBeBlank: "Användarnamn krävs",
            passwordCannotBeBlank: "Lösenord krävs",
            usernameOrPasswordIsInvalid: "Användarnamnet eller lösenordet är ogiltigt",
            emailCannotBeBlank: "E-Post krävs",
        };
        const languageLocale: ValidationLocaleStrings = {
            en: en,
            se: se,
        };
        return languageLocale;
    };

    getMessageStrings = () => {
        const en: MessageStrings = {
            resetEmailLinkSent: "Username is required",
        };

        const se: MessageStrings = {
            resetEmailLinkSent: "Användarnamn krävs",
        };
        
        const languageLocale: MessageLocaleStrings = {
            en: en,
            se: se,
        };
        return languageLocale;
    };

    
}