import { Component, OnInit } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';
import { FORM_DIRECTIVES, REACTIVE_FORM_DIRECTIVES, FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';

import { ApiService } from '../shared/api';

import './register.component.scss';

@Component({
    moduleId: module.id,
    selector: 'my-register',
    templateUrl: 'register.component.html',
    styleUrls: ['register.component.scss'],
    directives: [ROUTER_DIRECTIVES, FORM_DIRECTIVES, REACTIVE_FORM_DIRECTIVES],
    providers: [ApiService]
})
export class RegisterComponent implements OnInit {
    registerForm: FormGroup;
    error: string;
    success = false;
    loading = false;

    constructor(
        private api: ApiService,
        fb: FormBuilder
    ) {
        this.registerForm = fb.group({
            'username': ['', [Validators.required, Validators.maxLength(50)]],
            'firstName': ['', Validators.required],
            'lastName': ['', Validators.required],
            'password': ['', Validators.required],
            'confirmPassword': ['', c => this.validateConfirmPassword(c)]
        });

        this.registerForm.controls['password'].valueChanges.subscribe((value) => {
            this.registerForm.controls['confirmPassword'].updateValueAndValidity();
        });
    }

    ngOnInit() { }

    onSubmit(form: any) {
        this.error = '';
        this.success = false;
        this.loading = true;

        this.api.register(form)
            .finally(() => this.loading = false)
            .subscribe(
                r => this.onSuccess(),
                err => this.onError(err));
    }

    private onSuccess() {
        this.success = true;
        this.error = '';

        for (let controlName in this.registerForm.controls) {
            if (!this.registerForm.controls.hasOwnProperty(controlName)) {
                continue;
            }

            let control = (<FormControl>this.registerForm.controls[controlName]);
            control.updateValue('');
            control.setErrors(null);
        }
    }

    private onError(err: any) {
        this.error = err;
    }

    private validateConfirmPassword(c: FormControl) {
        if (this.registerForm && this.registerForm.controls['password'].value !== c.value) {
            return {
                confirm: {
                    valid: false
                }
            };
        }

        return null;
    }
}
