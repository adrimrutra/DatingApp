import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';



@Injectable()
export class ListsResolver implements Resolve<User[]> {
    pageNumber = 1;
    padeSize = 5;
    likesParam = 'Likers';

    resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
        return this.userService.getUsers(this.pageNumber, this.padeSize, null, this.likesParam).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving data!');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService) {}
}