/*var svg2png = require('svg2png');

function generateImage(fileName, size) {
    var sourceSize = 96.0;
    var scale = size / sourceSize;
    
    svg2png('../../Images/Icon00.svg', fileName, scale, function (err) { });
}

generateImage('Test.png', 128);
*/

var gm = require('gm');
var fs = require('fs');

var tempCounter = 0;

function generateImage(fileName, width, height) {
    var tempFile = 'temp' + ++tempCounter + '.png';
    
    gm('../../Images/Icon00.svg')
        .background('none')
        .density(600, 600)
        .negative()
        .resize(height)
        .write(tempFile, function (err) {
            if (err) {
                console.log(err);
                return;
            }
            
            var offset = (width - height) / 2;
            
            gm(tempFile)
                .background('#1593AC')
                .extent(width, height, ' -' + offset)
                .write(fileName, function (err) {
                    if (err) {
                        console.log(err);
                    }
                    
                    fs.unlink(tempFile);
                });
        });
}

var outputDir = '../TimesheetParser.Win10/Assets/';

generateImage(outputDir + 'LockScreenLogo.scale-200.png', 48, 48);
generateImage(outputDir + 'SplashScreen.scale-200.png', 1240, 600);
generateImage(outputDir + 'Square44x44Logo.scale-200.png', 88, 88);
generateImage(outputDir + 'Square44x44Logo.targetsize-24_altform-unplated.png', 24, 24);
generateImage(outputDir + 'Square150x150Logo.scale-200.png', 300, 300);
generateImage(outputDir + 'StoreLogo.png', 50, 50);
generateImage(outputDir + 'Wide310x150Logo.scale-200.png', 620, 300);
