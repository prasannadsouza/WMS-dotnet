import { Link } from 'react-router-dom'
import { AppSlice, useAppDispatch, useAppTrackedSelector, useTrackedGlobalState, useUpdateGlobalState } from '../../utilities/store';
import { LinkConstants } from '../../entities/constants'
import logo from '../../logo.svg';
import { Utility } from '../../utilities/utility';
import LocalizedStrings from 'react-localization';
import { Locale } from '../../utilities/locale';
import App from '../../App';

export const NavBar = () => {
    const updateAppConfig = useUpdateGlobalState();
    const appState = useTrackedGlobalState();

    const dispatch = useAppDispatch();
    const { setSessionData } = AppSlice.actions;

    const appData = useAppTrackedSelector();

    const isUserLoggedIn = Utility.isUserLoggedIn(appData);

    const getNavBrand = () => {
        let appLink = LinkConstants.LOGIN;

        if (isUserLoggedIn) {
            appLink = LinkConstants.HOME;
        }
        return (<Link className="btn navbar-brand m-0 p-0" to={Utility.getLink(appLink)}>
            <img src={logo} className="App-logo" style={{ height: '45px' }} alt="logo" />
            <strong className='pt-1'>{appData.applicationConfig.applicationTitle}</strong>
        </Link>);
    }

    const getNavLink = (title: string, key: string) => {
        return (
            <li className="nav-item">
                <Link
                    className="nav-link bg-secondary text-start text-white border-0 m-0"
                    to={Utility.getLink(key)}>
                    {title}
                </Link>
            </li>
        );
    }

    const changeLanguage = (code: string, e: any) => {
        let userLanguage = appData.languageCultures.filter(
            (language) => language.code === code
        )[0];

        var generalString = new LocalizedStrings(appData.generalLocaleString);
        generalString.setLanguage(code);
        var loginString = new LocalizedStrings(appData.loginLocaleString);
        loginString.setLanguage(code);

        updateAppConfig((prev) => (
            {
                ...prev,
                language: userLanguage,
                currentTitle: "",
                GeneralString: generalString,
                LoginString: loginString,
            }));
    };

    const performUserLogout = () => {
        let localeCode = appData.sessionData.user?.localeCode!;
        if (localeCode?.length > 0) appData.applicationConfig.localeCode = localeCode;

        let language = appData!.languageCultures!.filter(
            (language) => language.code === appData.applicationConfig.localeCode,
        )[0];

        dispatch(setSessionData(null));
        updateAppConfig((prev) => ({ ...prev, language: language }));
    }

    const OtherMenu = () => {

        const getRemainingMenu = () => {
            if (isUserLoggedIn !== true) return null;
            return (
                <li className="bg-secondary dropdown-item">
                    <button
                        className="btn bg-secondary text-white"
                        onClick={performUserLogout.bind(this)}
                    >
                        {appState.LoginString?.logout}
                    </button>
                </li>
            );
        };

        return (
            <div className='d-flex justify-content-end'>
                <button
                    className="dropdown-toggle bg-secondary border-0 text-white"
                    id="navbarLanguageDropdown"
                    data-bs-toggle="dropdown"
                    aria-expanded="false"
                >
                    {Utility.getOtherMenuTitle(appData, appState)}
                </button>
                <ul
                    className="dropdown-menu dropdown-menu-end p-0"
                    aria-labelledby="navbarLanguageDropdown">
                    {appData.languageCultures?.map((languageCulture) => {
                        return (
                            <li
                                className="bg-secondary dropdown-item"
                                key={languageCulture.code.toString()}
                            >
                                <button
                                    key={languageCulture.code}
                                    className="btn bg-secondary text-white"
                                    onClick={changeLanguage.bind(this, languageCulture.code)}
                                >
                                    {languageCulture.name}
                                </button>
                            </li>
                        );
                    })}
                    {getRemainingMenu()}
                </ul>
            </div>
        )
    }

    const getMainMenu = () => {

        if (isUserLoggedIn !== true) {
            return null;
        }

        return (
            <div className='pt-1'>
                <button
                    className="navbar-toggler"
                    type="button"
                    data-bs-toggle="dropdown"
                    id="navbarDropdown"
                >
                    <span className="navbar-toggler-icon"></span>
                </button>
                <ul
                    className="dropdown-menu nav-item  p-0 border-0"
                    aria-labelledby="navbarDropdown">
                    {getNavLink(appState.GeneralString!.home, LinkConstants.HOME)}
                    {getNavLink(appState.GeneralString!.settings, LinkConstants.SETTINGS)}
                    {getNavLink(appState.GeneralString!.fetchData, LinkConstants.FETCHDATA)}
                </ul>
            </div>);
    };

    return (
        <nav className=" navbar navbar-dark sticky-lg-top bg-secondary pb-0 pt-0 ps-1 pe-1">
            <section className="container-fluid w-100 p-0">
                <div className='col-sm-12 col-md-4 d-inline-flex'>
                    {getMainMenu()}
                    {getNavBrand()}
                </div>
                <div className='col-6 col-md-4 d-flex justify-content-start justify-content-sm-center pt-sm-1'>
                    <strong className='text-white'>{appState.currentTitle}</strong>
                </div>
                <div className='col-6 col-md-4 pt-sm-1'>
                    <OtherMenu />
                </div>
            </section>
        </nav>
    );
}; 