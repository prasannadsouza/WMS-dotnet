import { Language, GeneralString, GeneralLocaleString, LoginString, LoginLocaleString } from "../entities/locales"

export class Locale {

    getLanguages = () => {
        const languages: Language[] = [
            { id: 1, code: "en", name: "English" },
            { id: 2, code: "se", name: "Svenska" }
        ];
        return languages;
    };

    getGeneralString = () => {
        const en_SE: GeneralString = {
            home: "Home",
            settings: "Settings",
            language: "Language",
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
            fetchData: "Fetch Data",
            all: "All",
            to: "To",
            
        };

        const sv_SE: GeneralString = {
            home: "Hem",
            settings: "Installingar",
            language: "Språk",
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
            fetchData: "Fetch Data",
            all: "All",
            to: "To",
        };
        const languageLocale: GeneralLocaleString = {
            en_SE: en_SE,
            sv_SE: sv_SE,
        };
        return languageLocale;
    };

    getLoginString = () => {
        const en_SE: LoginString = {
            usernameCannotBeBlank: "Username is required",
            passwordCannotBeBlank: "Password is required",
            usernameOrPasswordIsInvalid: "Username or password is invalid",
            emailCannotBeBlank: "Email is required",
            resetEmailLinkSent: "Reset Email Link Sent",
            forgotPassword: "Forgot Password",
            sendPasswordResetLink: "Send Reset Link",
            login: "Login",
            logout: "Logout",
            username: "Username",
            password: "Password",
            loginTitle: "Login"
        };

        const sv_SE: LoginString = {
            usernameCannotBeBlank: "Användarnamn krävs",
            passwordCannotBeBlank: "Lösenord krävs",
            usernameOrPasswordIsInvalid: "Användarnamnet eller lösenordet är ogiltigt",
            emailCannotBeBlank: "E-Post krävs",
            resetEmailLinkSent: "Reset Email Link Sent",
            login: "Logga in",
            logout: "Logga ut",
            username: "Användarnamn",
            password: "Lösenord",
            forgotPassword: "Glömt lösenord",
            sendPasswordResetLink: "Skicka återställningslänk",
            loginTitle: "Logga in"
        };
        const languageLocale: LoginLocaleString = {
            en_SE: en_SE,
            sv_SE: sv_SE,
        };
        return languageLocale;
    };

    
}