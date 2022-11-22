import { Link } from 'react-router-dom'
import { AppSlice, useAppDispatch, useAppTrackedSelector, useTrackedGlobalState, useUpdateGlobalState } from '../../utilities/store';
import { APIParts, LinkConstants } from '../../entities/constants'
import logo from '../../logo.svg';
import { Utility } from '../../utilities/utility';
import LocalizedStrings from 'react-localization';
import { ResponseData } from '../../entities/entities';
import { useNavigate } from "react-router-dom";
import { Locale } from '../../utilities/locale';

export const NavBar = () => {
    const updateAppConfig = useUpdateGlobalState();
    const appState = useTrackedGlobalState();
    const navigate = useNavigate();

    const dispatch = useAppDispatch();
    const { setAppModel, setSessionData, setSessionLocale } = AppSlice.actions;

    const appData = useAppTrackedSelector();

    const isUserLoggedIn = Utility.isUserLoggedIn(appData);

    const changeLanguage = (code: string, e: any) => {
        let userLanguage = appData.languageCultures.filter(
            (language) => language.code === code
        )[0];
        
        dispatch(setSessionLocale(userLanguage));

        var generalString = new LocalizedStrings(appData.generalLocaleString);
        generalString.setLanguage(Locale.getLocalizedLocaleCode(code));
        updateAppConfig((prev) => (
            {
                ...prev,
                currentTitle: "",
                generalString: generalString,
            }));
    };

    const performUserLogout = () => {
        dispatch(setSessionData({ language: appData.sessionData.language }));
    }

    const reloadApp = async () => {
        Utility.clearConfigCache();
        await Utility.getAppData().then(newAppData => {

            if (Utility.handleErrors(appState, newAppData.appInitErrrors, updateAppConfig)) return;

            newAppData.sessionData = Utility.copyObjectData(appData.sessionData);
            dispatch(setAppModel(newAppData));

            const newAppState = Utility.getAppState(newAppData);
            updateAppConfig(() => (newAppState));
            navigate(Utility.getLink(LinkConstants.HOME));
        });
    }

    const performReloadApp = async () => {
        Utility.showLoader(updateAppConfig, true);
        await reloadApp().then(() => Utility.showLoader(updateAppConfig, false));
    }

    const clearServerCache = async () => {
        Utility.showLoader(updateAppConfig, true);
        await Utility.GetData<ResponseData<boolean>>(APIParts.CONFIG + "ClearCache", undefined, { data: null }).then(async (response) => {
            if (Utility.handleErrors(appState, response.errors, updateAppConfig)) return;
            await reloadApp().then(() => Utility.showLoader(updateAppConfig, false));
        });
    }

    const updateServerData = async () => {
        Utility.showLoader(updateAppConfig, true);
        await Utility.GetData<ResponseData<boolean>>(APIParts.CONFIG + "UpdateAllTimeStamp", undefined, { data: null }).then(async (response) => {
            if (Utility.handleErrors(appState, response.errors, updateAppConfig)) return;
            await reloadApp().then(() => Utility.showLoader(updateAppConfig, false));
        });
    }

    const getNavBrand = () => {
        let appLink = LinkConstants.LOGIN;

        if (isUserLoggedIn) {
            appLink = LinkConstants.HOME;
        }
        return (<Link className={(isUserLoggedIn ? "d-none d-xs-none d-sm-flex " : "") + "btn navbar-brand m-0 p-0"} to={Utility.getLink(appLink)}>
            <img src={logo} className="App-logo" style={{ height: '45px' }} alt="logo" />
            <strong className={(isUserLoggedIn ? "d-none d-xs-none d-sm-inline " : "") +  "pt-1"}>{appData.applicationConfig.applicationTitle}</strong>
        </Link>);
    }

    const getNavLink = (title: string, key: string) => {
        return (
            <li className="nav-item">
                <Link
                    className="nav-link bg-secondary text-start text-white border-0 m-0 ps-2 w-100"
                    to={Utility.getLink(key)}>
                    {title}
                </Link>
            </li>
        );
    }

    const OtherMenu = () => {

        const getRemainingMenu = () => {
            if (isUserLoggedIn !== true) return null;
            return (
                <li className="nav-item">
                    <button className="nav-link bg-secondary text-start text-white border-0 m-0 w-100 ps-2" onClick={performUserLogout.bind(this)}>
                        {appState.generalString?.logout}
                    </button>
                </li>
            );
        };

        const getOtherMenuTitle = () => {
            if (isUserLoggedIn !== true) {
                return (<span> {appData.sessionData.language.name} <i className="bi bi-caret-down-fill"></i></span>);
            }
            
            return (
                <div className="row m-0 p-0 ">
                    <div className="d-none d-sm-table col-10 small m-0 p-0">
                        <div className="text-nowrap">{appData.sessionData.user.firstName + " " + appData.sessionData.user.lastName }</div>
                        <div><small>{appData.sessionData.customer.name}</small></div>
                    </div>
                    <span className="col-2 m-0 p-0 ps-1 ms-auto me-2 me-sm-0"><i className="bi bi-person-lines-fill"></i><i className="bi bi-caret-down-fill"></i></span>
                </div>
            )
        }

        return (
            <div className='d-flex justify-content-end'>
                <button className="bg-secondary border-0 text-white" id="navbarLanguageDropdown" data-bs-toggle="dropdown" aria-expanded="false">
                    {getOtherMenuTitle()}
                </button>
                <ul
                    className="dropdown-menu dropdown-menu-end p-0"
                    aria-labelledby="navbarLanguageDropdown">
                    {appData.languageCultures?.map((languageCulture) => {
                        return (
                            <li className="nav-item" key={languageCulture.code.toString()}>
                                <button key={languageCulture.code} className="nav-link bg-secondary text-start text-white border-0 m-0 w-100 ps-2" onClick={changeLanguage.bind(this, languageCulture.code)}>
                                    {languageCulture.name}
                                </button>
                            </li>
                        );
                    })}
                    <li className="nav-item">
                        <button className="nav-link bg-secondary text-start text-white border-0 m-0 w-100 ps-2" onClick={performReloadApp.bind(this)}>
                            {appState.generalString?.reloadApp}
                        </button>
                    </li>
                    {getRemainingMenu()}
                </ul>
            </div>
        )
    }

    const getMainMenu = () => {

        if (isUserLoggedIn !== true) return null;

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
                    {getNavLink(appState.generalString!.home, LinkConstants.HOME)}
                    {getNavLink(appState.generalString!.settings, LinkConstants.SETTINGS)}
                    {getNavLink(appState.generalString!.fetchData, LinkConstants.FETCHDATA)}
                    <li className="nav-item">
                        <button className="nav-link bg-secondary text-start text-white border-0 m-0 ps-2 w-100" onClick={clearServerCache.bind(this)}>
                            {appState.generalString?.clearServerCache}
                        </button>
                    </li>
                    <li className="nav-item">
                        <button className="nav-link bg-secondary text-start text-white border-0 m-0 ps-2 w-100" onClick={updateServerData.bind(this)}>
                            {appState.generalString?.updateServerData}
                        </button>
                    </li>
                </ul>
            </div>);
    };

    return (
        <nav className=" navbar navbar-dark sticky-lg-top bg-secondary pb-0 pt-0 ps-1 pe-1">
            <section className="container-fluid w-100 p-0">
                <div className='col-sm-2 col-md-4 d-inline-flex'>
                    {getMainMenu()}
                    {getNavBrand()}
                </div>
                <div className='col-sm-5 col-md-4 d-flex justify-content-sm-end justify-content-md-center pt-sm-1'>
                    <strong className='text-white'>{appState.currentTitle}</strong>
                </div>
                <div className='col-sm-5 col-md-4 pt-sm-1'>
                    <OtherMenu />
                </div>
            </section>
        </nav>
    );
}; 

