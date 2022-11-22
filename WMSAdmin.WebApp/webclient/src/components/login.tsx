import { useAppTrackedSelector, useAppDispatch, useTrackedGlobalState, AppSlice, useUpdateGlobalState } from '../utilities/store';
import React, { useEffect, useRef } from 'react';
import { Utility } from '../utilities/utility';
import { UIHelper } from '../utilities/uihelper';
import { ErrorConstants, LinkConstants } from '../entities/constants';
import { useNavigate, Navigate } from "react-router-dom";
import { ErrorData, ResponseData } from '../entities/entities';
import { LoginModel } from "../entities/models";
import LocalizedStrings from 'react-localization';
import { Locale } from '../utilities/locale';
import { LoadingScreen } from './shared/loadingScreen';

export const Login = () => {
    const updateAppConfig = useUpdateGlobalState();
    const navigate = useNavigate();

    const dispatch = useAppDispatch();
    const { setSessionData, setLoginModel } = AppSlice.actions;

    const appData = useAppTrackedSelector();
    const model: LoginModel = appData.loginModel;

    const appState = useTrackedGlobalState();
    let loginString = model?.loginString;


    let passwordInput = useRef<HTMLInputElement>(null);
    let usernameInput = useRef<HTMLInputElement>(null);
    let emailInput = useRef<HTMLInputElement>(null);

    const getInitialLoginModel = (): LoginModel => {
        return {
            showPassword: false,
            showForgotPassword: false,
            email: "",
            emailFeedback: "",
            password: "",
            passwordFeedback: "",
            username: "",
            usernameFeedBack: "",
        };
    }

    let title = loginString?.loginTitle;
    if (model?.showForgotPassword === true) title = loginString?.forgotPassword

    const getCurrentModel = (): LoginModel => {
        return {
            email: model?.email,
            emailFeedback: model?.emailFeedback,
            password: model?.password,
            passwordFeedback: model?.passwordFeedback,
            showForgotPassword: model?.showForgotPassword,
            showPassword: model?.showPassword,
            username: model?.username,
            usernameFeedBack: model?.usernameFeedBack,
            loginString: model?.loginString,
        }
    }

    const resetPasswordHasErrors = (errors: ErrorData[]): boolean => {
        if ((errors?.length! > 0) === false) return false;
        let unhandledErrors: ErrorData[] = [];

        for (var error of errors!) {

            switch (error.errorCode) {
                case ErrorConstants.EMAIL_CANNOTBE_BLANK:
                    {
                        const currentModel = getCurrentModel();
                        currentModel.emailFeedback = error.message;
                        dispatch(setLoginModel(currentModel));
                        break;
                    }
                default:
                    {
                        unhandledErrors.push(error);
                    }
            }
        }

        if ((errors?.length! - unhandledErrors.length > 0)) return true;
        return Utility.handleErrors(appState, unhandledErrors, updateAppConfig, () => emailInput.current?.focus());

    };

    const loginHasErrors = (errors: ErrorData[]): boolean => {
        if ((errors?.length! > 0) === false) return false;
        let unhandledErrors: ErrorData[] = [];

        for (var error of errors!) {

            switch (error.errorCode) {
                case ErrorConstants.USERNAME_CANNOTBE_BLANK:
                    {
                        usernameInput.current?.focus();
                        let currentModel = getCurrentModel();
                        currentModel.usernameFeedBack = error.message;
                        dispatch(setLoginModel(currentModel));
                        break;
                    }
                case ErrorConstants.PASSWORD_CANNOTBE_BLANK:
                    {
                        passwordInput.current?.focus();
                        let currentModel = getCurrentModel();
                        currentModel.passwordFeedback = error.message;
                        dispatch(setLoginModel(currentModel));
                        break;
                    }
                default:
                    {
                        unhandledErrors.push(error);
                    }
            }
        }
        if ((errors?.length! - unhandledErrors.length > 0)) return true;
        return Utility.handleErrors(appState, unhandledErrors, updateAppConfig, () => usernameInput.current?.focus());
    };

    const handleForgotPasswordClick = (event: React.MouseEvent) => {
        event.preventDefault();
        performResetPassword();
    }

    const handleForgotPasswordKeyEvent = (event: React.KeyboardEvent) => {
        if (event.key.toLowerCase() !== "enter") return;
        const { name } = event.target as HTMLInputElement;

        if (name === passwordInput.current?.name) {
            event.preventDefault();
            performResetPassword();
            return;
        }
    }

    const performResetPassword = () => {
        let currentModel = getCurrentModel();
        currentModel.emailFeedback = "";
        dispatch(setLoginModel(currentModel));

        const response = validateResetPassword();
        if (resetPasswordHasErrors(response?.errors)) return;

        const onClose = (confirmed: boolean) => {
            if (confirmed) {
                showLogin(true);
                UIHelper.showMessageModal(updateAppConfig,appState, false, "Reset Email Sent",);
                return;
            }
            else {
                emailInput.current?.focus();
            }
        };
        UIHelper.showConfirmationModal(updateAppConfig, appState, onClose);
    };

    const handleLoginClick = (event: React.MouseEvent) => {
        event.preventDefault();
        performUserLogin();
    }

    const validateUserLogin = (): ResponseData<boolean> => {

        let response: ResponseData<boolean> = {
            errors: []
        }

        if ((model.username?.trim()?.length > 0) !== true) {
            response.errors?.push({
                errorCode: ErrorConstants.USERNAME_CANNOTBE_BLANK,
                message: model.loginString?.usernameCannotBeBlank,
            });
        }

        if ((model.password?.trim()?.length > 0) !== true) {
            response.errors?.push({
                errorCode: ErrorConstants.PASSWORD_CANNOTBE_BLANK,
                message: model.loginString?.passwordCannotBeBlank,
            });
        }

        if (model.username === "prasanna") {
            response.errors?.push({
                errorCode: ErrorConstants.USERNAME_OR_PASSWORD_ISINVALID,
                message: model.loginString?.usernameOrPasswordIsInvalid,
            });
        }

        if ((response.errors!.length > 0) === true) return response;
        response.data = true;
        return response;
    }


    const validateResetPassword = (): ResponseData<boolean> => {
        let response: ResponseData<boolean> = {
            data: false,
            errors: []
        }

        if ((model.email?.trim()?.length > 0) !== true) {
            response.errors?.push({
                errorCode: ErrorConstants.EMAIL_CANNOTBE_BLANK,
                message: model.loginString?.emailCannotBeBlank,
            });
        }

        response.data = true;
        return response;

    }

    const performUserLogin = () => {

        let newModel = getCurrentModel();
        newModel.passwordFeedback = "";
        newModel.usernameFeedBack = "";
        dispatch(setLoginModel(newModel));

        const response = validateUserLogin();
        if (loginHasErrors(response?.errors)) return;

        let sessionData = Utility.getSessionConfig(appData, model.username!, model.password!);
        dispatch(setSessionData(sessionData));
        newModel = getInitialLoginModel();
        newModel.loginString = model.loginString;
        dispatch(setLoginModel(newModel));

        updateAppConfig(() => (Utility.getAppState(appData)));
        navigate(Utility.getLink(LinkConstants.HOME));
    }

    const handleLoginKeyEvent = (event: React.KeyboardEvent) => {
        if (event.key.toLowerCase() !== "enter") return;
        const { name } = event.target as HTMLInputElement;

        if (name === usernameInput?.current?.name) {
            event.preventDefault();
            passwordInput.current!.focus();
            return;
        }

        if (name === passwordInput.current?.name) {
            event.preventDefault();
            performUserLogin();
            return;
        }
    };

    const showForgotPassword = (resetFields: boolean) => {

        let currentModel = getCurrentModel();
        currentModel.showForgotPassword = true;

        if (resetFields) {
            currentModel.emailFeedback = "";
        }
        dispatch(setLoginModel(currentModel));
        updateAppConfig((prev) => ({ ...prev, currentTitle: loginString.forgotPassword }));

    };

    const showLogin = (resetFields: boolean) => {

        let currentModel = getCurrentModel();
        currentModel.showForgotPassword = false;
        if (resetFields) {
            currentModel.username = "";
            currentModel.password = "";
            currentModel.emailFeedback = "";
            currentModel.passwordFeedback = "";
            currentModel.email = "";
        }
        dispatch(setLoginModel(currentModel));
        updateAppConfig((prev) => ({ ...prev, currentTitle: loginString.login }));

    };

    const setLanguage = async (newModel: LoginModel) => {
        await Locale.getLoginString().then(response => {
            if (Utility.handleErrors(appState, response?.errors, updateAppConfig)) return;

            newModel.loginString = new LocalizedStrings(response.data);
            newModel.loginString.setLanguage(Locale.getLocalizedLocaleCode(appData.sessionData.language.code))
            dispatch(setLoginModel(newModel))
            updateAppConfig((prev) => ({ ...prev, currentTitle: newModel.loginString.loginTitle, showLoader: false }));
        });
    }

    const setModel = async () => {
        await setLanguage(getInitialLoginModel());
    };

    useEffect(() => {

        if (model === undefined) {
            updateAppConfig((prev) => ({ ...prev, showLoader: true }));
            setModel();
        }

        if (Utility.isUserLoggedIn(appData)) {
            navigate(Utility.getLink(LinkConstants.HOME), { replace: true });
            return;
        }

        updateAppConfig((prev) => ({ ...prev, currentTitle: title }));
    }, []);

    useEffect(() => {

        if (model !== undefined) {
            setLanguage(getCurrentModel());
        }

        updateAppConfig((prev) => ({ ...prev, currentTitle: title }));
    }, [appData.sessionData.language]);


    const renderLogin = () => {
        return (
            <section className='col-12 col-md-8 col-lg-6 bg-white  border border-warning rounded-3 p-2'>
                <fieldset className='col-12 fieldsetLabel'>
                    <label>{loginString.username}<span className='text-danger'> *</span></label>
                    <div className={"input-group border rounded" + UIHelper.getclassIsInvalid(model?.usernameFeedBack!)}>
                        <input name="username" ref={usernameInput} type="text" className={"form-control border-0 me-1 rounded" + UIHelper.getclassIsInvalid(model?.usernameFeedBack!)}
                            onChange={(e) => {
                                // setModelData({ ...model, username: e.target.value, usernameFeedBack: "" }) 
                                let currentModel = getCurrentModel();
                                currentModel.username = e.target.value;
                                currentModel.usernameFeedBack = "";
                                dispatch(setLoginModel(currentModel));
                            }}
                            value={model?.username}
                            onKeyDown={(e) => handleLoginKeyEvent(e)} />
                        <div className="input-group-append">
                            <button tabIndex={-1} className="btn btn-outline-secondary border-0"
                                onClick={(e) => {
                                    //setModelData({ ...model, username: "", usernameFeedBack: "" })
                                    let currentModel = getCurrentModel();
                                    currentModel.username = "";
                                    currentModel.usernameFeedBack = "";
                                    dispatch(setLoginModel(currentModel));

                                }}
                                hidden={(model?.username?.length! > 0) === false}><i className="bi bi-x-circle-fill"></i></button>
                        </div>
                    </div>
                    <div className="invalid-feedback">{model?.usernameFeedBack}</div>
                </fieldset>
                <fieldset className='col-12 fieldsetLabel'>
                    <label>{loginString.password}<span className='text-danger'> *</span></label>
                    <div className={"input-group border rounded" + UIHelper.getclassIsInvalid(model?.passwordFeedback!)}>
                        <input name="password" ref={passwordInput} type={model?.showPassword ? "text" : "password"}
                            className={"form-control border-0 rounded" + UIHelper.getclassIsInvalid(model?.passwordFeedback!)}
                            onChange={(e) => {
                                //setModelData({ ...model, password: e.target.value, passwordFeedback: "" })
                                let currentModel = getCurrentModel();
                                currentModel.password = e.target.value;
                                currentModel.passwordFeedback = "";
                                dispatch(setLoginModel(currentModel));
                            }

                            } value={model?.password}
                            onKeyDown={(e) => handleLoginKeyEvent(e)} />
                        <div className="input-group-append">
                            <button tabIndex={-1} className="btn btn-outline-secondary  border-0"
                                onClick={(e) => {
                                    //setModelData({ ...model, showPassword: !model.showPassword })
                                    let currentModel = getCurrentModel();
                                    currentModel.showPassword = !model?.showPassword;
                                    dispatch(setLoginModel(currentModel));
                                }}
                            ><i className={"bi " + (model?.showPassword ? "bi-eye-fill" : "bi-eye-slash-fill")}></i></button>
                        </div>
                        <div className="input-group-append">
                            <button tabIndex={-1} className="btn btn-outline-secondary border-0"
                                onClick={(e) => {
                                    //setModelData({ ...model, password: "", passwordFeedback: "" })
                                    let currentModel = getCurrentModel();
                                    currentModel.password = "";
                                    currentModel.passwordFeedback = "";
                                    dispatch(setLoginModel(currentModel));
                                }}

                                hidden={(model?.password?.length! > 0) === false} ><i className="bi bi-x-circle-fill"></i></button>
                        </div>
                    </div>
                    <div className="invalid-feedback">{model?.passwordFeedback}</div>
                </fieldset>
                <div className="mt-3 d-flex justify-content-around">
                    <button className='btn btn-secondary' onClick={(e) => showForgotPassword(false)} >{loginString?.forgotPassword}</button>
                    <button className='btn btn-primary' onClick={(e) => handleLoginClick(e)} >{loginString?.login}</button>
                </div>
            </section>
        );
    }

    const renderForgotPassword = () => {
        return (
            <section className='col-12 col-md-8 col-lg-6 bg-white  border border-warning rounded-3 p-2'>
                <fieldset className='col-12 fieldsetLabel'>
                    <label>{appState.generalString.email}<span className='text-danger'> *</span></label>
                    <div className={"input-group border rounded" + UIHelper.getclassIsInvalid(model?.emailFeedback!)}>
                        <input name="email" ref={emailInput} type="text" className={'form-control border-0 me-1 rounded' + UIHelper.getclassIsInvalid(model?.emailFeedback!)}
                            onChange={(e) => {
                                let currentModel = getCurrentModel();
                                currentModel.email = e.target.value;
                                currentModel.emailFeedback = "";
                                dispatch(setLoginModel(currentModel));
                                //setModelData({ ...model, email: e.target.value, emailFeedback: "" })
                            }}
                            value={model?.email}
                            onKeyDown={(e) => handleForgotPasswordKeyEvent(e)} />
                        <div className="input-group-append">
                            <button tabIndex={-1} className="btn btn-outline-secondary border-0"
                                onClick={(e) => {

                                    //setModelData({ ...model, email: "", emailFeedback: "" }) }
                                    let currentModel = getCurrentModel();
                                    currentModel.email = "";
                                    currentModel.emailFeedback = "";
                                    dispatch(setLoginModel(currentModel));
                                }
                                }
                                hidden={(model?.email?.length! > 0) === false}><i className="bi bi-x-circle-fill"></i></button>
                        </div>
                    </div>
                    <div className="invalid-feedback">{model?.emailFeedback}</div>
                </fieldset>
                <div className="mt-3 d-flex justify-content-around">
                    <button className='btn btn-secondary' onClick={(e) => showLogin(false)}>{appState.generalString.cancel}</button>
                    <button className='btn btn-primary' onClick={(e) => handleForgotPasswordClick(e)} >{loginString.sendPasswordResetLink}</button>
                </div>
            </section>
        );
    }

    const renderForm = () => {

        if (Utility.isUserLoggedIn(appData)) {
            return (<Navigate to={Utility.getLink(LinkConstants.HOME)} />);
        }

        const renderContent = () => {
            if (model === undefined) return (<LoadingScreen />)
            if (model?.showForgotPassword === true) return renderForgotPassword();
            return renderLogin();
        }

        return (
            <div className='container' >
                <div className='row mt-5'>
                    <section className="col-12 col-md-2 col-lg-3"></section>
                    {renderContent()}
                    <section className="col-12 col-md-2 col-lg-3"></section>
                </div>
            </div>
        );
    }

    return (renderForm());
}