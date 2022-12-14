//
//  AppLovinAd.h
//  sdk
//
//  Copyright (c) 2013, AppLovin Corporation. All rights reserved.


#import <Foundation/Foundation.h>
#import "ALAnnotations.h"

#import "ALAdSize.h"
#import "ALAdType.h"

/**
 * This class represents an ad that has been served from the AppLovin server and
 * should be displayed to the user.
 */
@interface ALAd : NSObject <NSCopying>

/**
 * @name Ad Properties
 */

/**
 *  The size of this ad.
 */
@property (strong, nonatomic, readonly) ALAdSize * __alnonnull size;

/**
 *  The type of this ad.
 */
@property (strong, nonatomic, readonly) ALAdType * __alnonnull type;

/**
 *  Whether or not the current ad is a video advertisement.
 */
@property (assign, readonly, getter=isVideoAd) BOOL videoAd;

/**
 * @name Ad Identification
 */

/**
 *  A unique ID which identifies this advertisement.
 *  
 *  Should you need to report a broken ad to AppLovin support, please include this number's longValue.
 */
@property (strong, nonatomic, readonly) NSNumber * __alnonnull adIdNumber;

// These property aliases are left for backwards compatibility only, and should no longer be used.
@property (strong, readonly, getter=size) ALAdSize * __alnullable adSize __deprecated_msg("Use size property instead.");
@property (strong, readonly, getter=type) ALAdType * __alnullable adType __deprecated_msg("Use type property instead.");

@end
