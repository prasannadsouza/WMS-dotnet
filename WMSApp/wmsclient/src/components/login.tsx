import { useAppTrackedSelector, useAppDispatch, useTrackedGlobalState, AppSlice, useUpdateGlobalState } from '../utilities/store';
import React, { useEffect, useRef } from 'react';
import { Utility } from '../utilities/utility';
import { UIHelper } from '../utilities/uihelper';
import { ResponseModel } from '../entities/models';
import { ErrorConstants, LinkConstants } from '../entities/constants';
import { useNavigate, Navigate } from "react-router-dom";
import { ErrorData } from '../entities/entities';
import { LoginModel } from "../entities/models";


export const Login = () => {
    const updateAppConfig = useUpdateGlobalState();
    const navigate = useNavigate();
    
    const dispatch = useAppDispatch();
    const { setAppModel, setLoginModel } = AppSlice.actions;

    const appData = useAppTrackedSelector();
    const appConfig = appData.appConfig;
    const model: LoginModel = appData.loginModel;

    const appState = useTrackedGlobalState();
    const globalStrings = appState.globalStrings;

    let passwordInput = useRef<HTMLInputElement>(null);
    let usernameInput = useRef<HTMLInputElement>(null);
    let emailInput = useRef<HTMLInputElement>(null);

    let title = globalStrings!.login;
    if (model?.showForgotPassword === true) title = globalStrings!.forgotPassword

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
        }
    }

    const resetPasswordHasErrors = <T,>(response: ResponseModel<T>): boolean => {
        if ((response?.errors?.length! > 0) === false) return false;
        let unhandledErrors: ErrorData[] = [];

        for (var error of response.errors!) {

            switch (error.errorCode) {
                case ErrorConstants.EMAIL_CANNOTBE_BLANK:
                    {
                        let currentModel = getCurrentModel();
                        currentModel.emailFeedback = error.message;
                        dispatch(setLoginModel(currentModel));
                        //setModelData({ ...model, emailFeedback: error.message ?? "" })
                        break;
                    }
                default:
                    {
                        unhandledErrors.push(error);
                    }
            }
        }

        if ((response.errors?.length! - unhandledErrors.length > 0)) return true;
        if ((unhandledErrors.length > 0) === false) return true;
        let messageModel = Utility.getMessageModel(appState);
        messageModel.message = unhandledErrors[0].message;
        messageModel.show = true;
        messageModel.isError = true;
        messageModel.title = globalStrings?.error;
        messageModel!.onClose = () => emailInput.current?.focus();
        updateAppConfig((prev) => ({ ...prev, messageModel: messageModel }));
        return true;
    };

    let loginHasErrors = <T,>(response: ResponseModel<T>): boolean => {
        if ((response?.errors?.length! > 0) === false) return false;
        let unhandledErrors: ErrorData[] = [];

        for (var error of response.errors!) {

            switch (error.errorCode) {
                case ErrorConstants.USERNAME_CANNOTBE_BLANK:
                    {
                        usernameInput.current?.focus();
                        //setModelData({ ...model, usernameFeedBack: error.message ?? "" })
                        let currentModel = getCurrentModel();
                        currentModel.usernameFeedBack = error.message;
                        dispatch(setLoginModel(currentModel));
                        break;
                    }
                case ErrorConstants.PASSWORD_CANNOTBE_BLANK:
                    {
                        passwordInput.current?.focus();
                        //setModelData({ ...model, passwordFeedback: error.message ?? "" })
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


        if ((response.errors?.length! - unhandledErrors.length > 0)) return true;
        if ((unhandledErrors.length > 0) === false) return true;
        let messageModel = Utility.getMessageModel(appState);
        messageModel.message = unhandledErrors[0].message;
        messageModel.show = true;
        messageModel.isError = true;
        messageModel.title = globalStrings?.error;
        messageModel!.onClose = () => {
            usernameInput.current?.focus();
        };
        updateAppConfig((prev) => ({ ...prev, messageModel: messageModel }));
        return true;
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
        //setModelData({ ...model, emailFeedback: "" })
        let currentModel = getCurrentModel();
        currentModel.emailFeedback = "";
        dispatch(setLoginModel(currentModel));

        const response = Utility.validateResetPassword(appState, model.email!);
        if (resetPasswordHasErrors(response)) return;

        let confirmModel = Utility.getConfirmModel(appState);
        confirmModel.show = true;
        confirmModel!.onClose = (confirmed: boolean) => {
            if (confirmed) {
                showLogin(true);
                updateAppConfig((prev) => ({ ...prev, confirmModel: Utility.getConfirmModel(appState) }));
            }
            else {
                emailInput.current?.focus();
            }
        };
        updateAppConfig((prev) => ({ ...prev, confirmModel: confirmModel }));
    };

    const handleLoginClick = (event: React.MouseEvent) => {
        event.preventDefault();
        performUserLogin();
    }

    const performUserLogin = () => {
        let currentModel = getCurrentModel();
        currentModel.passwordFeedback = "";
        currentModel.usernameFeedBack = "";
        dispatch(setLoginModel(currentModel));

        const response = Utility.performUserLogin(appState, model.username!, model.password!);
        if (loginHasErrors(response)) return;
        var newAppConfig = Utility.getAppConfig();
        newAppConfig.session = Utility.getSessionConfig(model.username!, model.password!);
        dispatch(setAppModel(newAppConfig));
        dispatch(setLoginModel(UIHelper.getInitialLoginModel()));

        var newAppState = Utility.getAppState(newAppConfig,appState);
        updateAppConfig(() => (newAppState));
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
            //setModelData({ ...model, showForgotPassword: true, email: "", emailFeedback: "" });
            currentModel.emailFeedback = "";
            dispatch(setLoginModel(currentModel));
        }
        else {
            //setModelData({ ...model, showForgotPassword: true });
            dispatch(setLoginModel(currentModel));
        }

        updateAppConfig((prev) => ({ ...prev, currentTitle: globalStrings!.forgotPassword }));

    };

    const showLogin = (resetFields: boolean) => {

        let currentModel = getCurrentModel();
        currentModel.showForgotPassword = false;
        if (resetFields) {
            //setModelData({ ...model, showForgotPassword: false, username: "", password: "", emailFeedback: "", passwordFeedback: "" });
            currentModel.username = "";
            currentModel.password = "";
            currentModel.emailFeedback = "";
            currentModel.passwordFeedback = "";
        }
        else {
            //setModelData({ ...model, showForgotPassword: false });

        }
        dispatch(setLoginModel(currentModel));
        updateAppConfig((prev) => ({ ...prev, currentTitle: globalStrings!.login }));
        
    };

    useEffect(() => {
        if (Utility.isUserLoggedIn(appConfig)) {
            navigate(Utility.getLink(LinkConstants.HOME), { replace: true });
            return;
        }
        updateAppConfig((prev) => ({ ...prev, currentTitle: title }));
    }, []);

    useEffect(() => {
        updateAppConfig((prev) => ({ ...prev, currentTitle: title }));
    }, [appState.currentTitle]);


    const renderLogin = () => {
        return (
            <section className='col-12 col-md-5 col-lg-4 bg-white  border border-warning rounded-3 p-2'>
                <fieldset className='col-12 fieldsetLabel'>
                    <label>{globalStrings!.username}<span className='text-danger'> *</span></label>
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
                    <label>{globalStrings!.password}<span className='text-danger'> *</span></label>
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
                    <button className='btn btn-secondary' onClick={(e) => showForgotPassword(false)} >{globalStrings?.forgotPassword}</button>
                    <button className='btn btn-primary' onClick={(e) => handleLoginClick(e)} >{globalStrings?.login}</button>
                </div>
            </section>
        );
    }

    const renderForgotPassword = () => {
        return (
            <section className='col-12 col-md-6 col-lg-4 bg-white  border border-warning rounded-3 p-2'>
                <fieldset className='col-12 fieldsetLabel'>
                    <label>{globalStrings!.email}<span className='text-danger'> *</span></label>
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
                    <button className='btn btn-secondary' onClick={(e) => showLogin(false)}>{globalStrings?.cancel}</button>
                    <button className='btn btn-primary' onClick={(e) => handleForgotPasswordClick(e)} >{globalStrings?.sendPasswordResetLink}</button>
                </div>
            </section>
        );
    }

    const renderForm = () => {

        if (Utility.isUserLoggedIn(appConfig)) {
            return (<Navigate to={Utility.getLink(LinkConstants.HOME)} />);
        }

        return (
            <div className='container' >
                <div className='row mt-5'>
                    <section className="col-12 col-md-3 col-lg-4"></section>
                    {model?.showForgotPassword === true ? renderForgotPassword() : renderLogin()}
                    <section className="col-12 col-md-3 col-lg-4"></section>
                </div>
            </div>
        );
    }

    return (renderForm());
}